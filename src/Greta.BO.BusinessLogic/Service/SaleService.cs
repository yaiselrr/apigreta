using System;
using System.Collections.Generic;
using System.Globalization;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Entities.Dto;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.Api.Sqlserver;
using Microsoft.VisualBasic;
using Greta.BO.BusinessLogic.Extensions;
using Microsoft.Extensions.Configuration;

namespace Greta.BO.BusinessLogic.Service
{
    public interface ISaleService : IGenericBaseService<Sale>
    {
        Task FixDuplicationSales();
        Task<Sale> GetByLocalId(long id);
        Task<Sale> GetByInvoice(string id);

        Task<List<ClosableElementModel>> GetCloseableElementsByStoreAndDate(long storeId, DateTime initDate,
            DateTime endDate);

        Task<ProcessEndOfDayResponse> GetEndOfDayResume(long storeId, DateTime initDate,
            DateTime endDate);

        Task<ProcessEndOfDayResponse> ProcessEndOfDay(long storeId, long element, EndOfDayHolder holder,
            DateTime initDate,
            DateTime endDate, bool persist = false);

        Task<bool> ExistSale(string invoice, DateTime saleTme, decimal subTotal);
        // Task FixSaleTenderDataCards();

        Task ProcessQty(List<StoreProduct> qtySales, long storeId);

        Task<SalesByHourResponse> GetSalesByHour(int begin_hour, int end_hour);
        Task<SalesByHourResponse> GetSalesByHourAndStore(int begin_hour, int end_hour, long storeId, string timezoneId);
        Task<SalesByHourResponse> GetSalesByWeek(long storeId);
        Task<List<SalesSearchResponseDetailDto>> GetPosFilteredSale(Device device, SalesSearchRequestDto filter);
    }

    public class SaleService : BaseService<ISaleRepository, Sale>, ISaleService
    {
        private readonly IConfiguration configuration;

        public SaleService(
            ISaleRepository repository,
            IConfiguration configuration,
            ILogger<SaleService> logger)
            : base(repository, logger)
        {
            this.configuration = configuration;
        }
        
        
        /// <summary>
        ///     Get entity by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Customer</returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        public override async Task<Sale> Get(long id)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds.");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            var entity = await _repository.GetEntity<Sale>()
                .Include(x => x.Products)
                //.Include(x => x.Tax)
                .Include(x => x.Discounts)
                .Include(x => x.Tenders)
                .Include(x => x.Fees)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            return entity;
        }

        public async Task<Sale> GetByLocalId(long id)
        {
            return await _repository.GetEntity<Sale>()
                .Where(x => x.LocalId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Sale> GetByInvoice(string id)
        {
            return await _repository.GetEntity<Sale>()
                .Where(x => x.Invoice == id)
                .FirstOrDefaultAsync();
        }

        public async Task ProcessQty(List<StoreProduct> qtySales, long storeId)
        {
            var b = new DbContextOptionsBuilder()
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection")
                    , sqlopt =>
                    {
                        sqlopt.UseAdminDatabase("defaultdb");
                        sqlopt.EnableRetryOnFailure(10, TimeSpan.FromSeconds(10),
                            null);
                    });

            using (var context = new SqlServerContext(b.Options))
            {
                foreach (var s in qtySales)
                {
                    var store = await context.Set<StoreProduct>()
                        .Include(x => x.Parent)
                        .Where(x => x.StoreId == storeId && x.ProductId == s.ProductId)
                        .FirstOrDefaultAsync();
                    if (store != null)
                    {
                        if (store.ParentId == null || store.QtyHand >= s.QtyHand || store.SplitCount == 0) 
                        {
                            store.QtyHand -= s.QtyHand;
                            context.Set<StoreProduct>().Update(store);
                            //_logger.LogInformation($"Product {store.ProductId} have {s.QtyHand} substracted.");
                        }
                        else
                        {
                            //process inventory throw breakpacks
                            await UpdateBreakPack(context, store, s.QtyHand);
                        }
                        
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        private async Task RefillFromBottom(SqlServerContext context, StoreProduct product)
        {
            if (product.ParentId != null && product.Parent.QtyHand > 1)
            {
                product.Parent.QtyHand--;
                product.QtyHand = product.SplitCount;
                context.Set<StoreProduct>().Update(product);
            }
            else if (product.ParentId != null && product.Parent.QtyHand == 1)
            {
                product.Parent.QtyHand--;
                product.QtyHand = product.SplitCount;
                context.Set<StoreProduct>().Update(product);
                var rParent = await context.Set<StoreProduct>()
                    .Include(x => x.Parent)
                    .Where(x => x.Id == product.ParentId)
                    .FirstOrDefaultAsync();
                await RefillFromBottom(context, rParent);
            }
        }
        
        public  async Task<decimal> UpdateBreakPack(SqlServerContext context, StoreProduct product, decimal amount)
        {
            if (product.QtyHand >= amount || product.ParentId == null)
            {
                product.QtyHand -= amount;
                
                if (product.QtyHand == 0)
                {
                    // if (product.ParentId != null && product.Parent.QtyHand > 0)
                    // {
                    //     product.Parent.QtyHand--;
                    //     product.QtyHand = product.SplitCount;
                    // }
                    await RefillFromBottom(context, product);
                }
                else
                {
                    context.Set<StoreProduct>().Update(product);
                }
                return product.QtyHand;
            }
            else
            {
                var nAmount = amount - product.QtyHand;
                product.QtyHand = 0M;
                //Calulate de top multiple of splitCount example fro 11 is 20 if splitcount is 10
                int newParentCount = (int)((nAmount + product.SplitCount - 1) / product.SplitCount);
                var rParent = await context.Set<StoreProduct>()
                    .Include(x => x.Parent)
                    .Where(x => x.Id == product.ParentId)
                    .FirstOrDefaultAsync();
                    
                var remainInParent = await UpdateBreakPack(context, rParent, newParentCount);
                if (remainInParent > 0)
                {
                    product.QtyHand = newParentCount * product.SplitCount - nAmount;
                    context.Set<StoreProduct>().Update(product);
                }else if (remainInParent == 0)
                {
                    if (newParentCount * product.SplitCount - nAmount == 0)
                    {
                        //manage the refill
                        if (rParent.ParentId != null && rParent.Parent.QtyHand > 0)
                        {
                            rParent.Parent.QtyHand--;
                            product.QtyHand = product.SplitCount;
                            context.Set<StoreProduct>().Update(product);
                        }
                        else
                        {
                            product.QtyHand = newParentCount * product.SplitCount - nAmount;
                            context.Set<StoreProduct>().Update(product);
                        }
                    }
                }

                return product.QtyHand ;
            }
        }
        
        public async Task<List<ClosableElementModel>> GetCloseableElementsByStoreAndDate(long storeId,
            DateTime initDate, DateTime endDate)
        {
            var store = await _repository.GetEntity<Store>().Where(x => x.Id == storeId).FirstOrDefaultAsync();
            var holders = new List<long>();
            var result = new List<ClosableElementModel>();
            if (store.DrawerTraking == DrawerTraking.ByCashier)
            {
                var sales = await _repository.GetEntity<Sale>()
                    // .Include(x => x.Employee)
                    .Where(x => x.StoreId == storeId && x.SaleTime >= initDate && x.SaleTime <= endDate &&
                                x.EndOfDayId == null)
                    .ToListAsync();

                foreach (var s in sales)
                {
                    if (!holders.Contains(s.EmployeeId))
                    {
                        holders.Add(s.EmployeeId);
                        result.Add(new ClosableElementModel() { Id = s.EmployeeId, Name = s.Username });
                    }
                }
            }
            else
            {
                var sales = await _repository.GetEntity<Sale>()
                    // .Include(x => x.Device)
                    .Where(x => x.StoreId == storeId && x.SaleTime >= initDate && x.SaleTime <= endDate &&
                                x.EndOfDayId == null)
                    .ToListAsync();

                foreach (var s in sales)
                {
                    if (!holders.Contains(s.DeviceId))
                    {
                        holders.Add(s.DeviceId);
                        result.Add(new ClosableElementModel() { Id = s.DeviceId, Name = s.Username });
                    }
                }
            }

            return result;
        }

        //public async Task FixOldEndOfDay()
        //{
        //    var ends = await _repository.GetEntity<EndOfDay>()
        //        .OrderBy(x => x.SaleDay)
        //        .ToListAsync();
        //    foreach(var e in ends)
        //    {
        //        var sales = await _repository.GetEntity<Sale>()
        //            .Include(x => x.Products)
        //            .Include(x => x.Tenders)
        //            .Where(x => x.EndOfDayId == e.Id)
        //           .ToListAsync();

        //        //Total sales using tender
        //        var totalSales = 
        //            sales.Where(x => x.Tenders.Any(t => t.Amount > 0)).Sum(x => x.Tenders.Sum(t => (t.Name != "Cash" ? t.ApprovedAmount : t.Amount) - t.CashBack));

        //        var tenderedCashTotal = sales.Sum(x => x.Tenders.Where(t => t.Name == "Cash" && t.Amount > 0).Sum(t => t.Amount - t.CashBack));

        //        var bottleReturnTotal = sales.Sum(x => x.Products.Where(p => p.Name == "Bottle Refund").Sum(t => t.TotalPrice));

        //        var refundReturnCash = sales.Where(s => s.Products.Any(x => x.Name == "Return"))
        //            .Sum(x => x.Tenders.Where(p => p.Name == "Cash").Sum(t => t.Amount));

        //        var refundReturnOthers = sales.Where(s => s.Products.Any(x => x.Name == "Return"))
        //            .Sum(x => x.Tenders.Where(p => p.Name != "Cash" && p.Name != "Snap/EBT").Sum(t => t.Amount));

        //        var refundReturnSnapEbt = sales.Where(s => s.Products.Any(x => x.Name == "Return"))
        //            .Sum(x => x.Tenders.Where(p => p.Name == "Snap/EBT").Sum(t => t.Amount));

        //        //paid out
        //        var paidOutCash = sales.Where(s => s.Products.Any(x => x.Name == "Paid Out"))
        //            .Sum(x => x.Tenders.Where(p => p.Name == "Cash").Sum(t => t.Amount));

        //        var creditCardSales = sales.Sum(x => x.Tenders.Where(t => t.CardType == "Credit Card" && t.Amount > 0).Sum(t => t.ApprovedAmount));

        //        e.TenderedCashTotal = tenderedCashTotal;

        //        e.TotalSales = totalSales;

        //        e.BottleReturnTotal = Math.Abs(bottleReturnTotal);
        //        e.RefundReturn = Math.Abs(refundReturnCash);
        //        e.RefundReturnEbt = Math.Abs(refundReturnSnapEbt);
        //        e.RefundReturnOther = Math.Abs(refundReturnOthers);
        //        e.PaidOut = Math.Abs(paidOutCash);

        //        e.TotalCash =
        //            e.TenderedCashTotal -
        //            (e.BottleReturnTotal + e.DebitCashBack + e.RefundReturn +
        //             e.EBTCashBack + e.PaidOut); 

        //        e.CreditCardSales = creditCardSales;

        //        e.CashTotalCounted =
        //            (e.Count100 * 100) +
        //            (e.Count50 * 50) +
        //            (e.Count20 * 20) +
        //            (e.Count10 * 10) +
        //            (e.Count5 * 5) +
        //            (e.Count1 + e.Countc100) +
        //            (e.Countc50 * 0.5m) +
        //            (e.Countc25 * 0.25m) +
        //            (e.Countc10 * 0.10m) +
        //            (e.Countc5 * 0.05m);
        //        e.CashToDeposit = e.CashTotalCounted - e.StartingCash;
        //        e.CashOverShort = e.CashToDeposit - e.TotalCash;// here we need add total cash

        //        _repository.GetEntity<EndOfDay>().Update(e);
        //    }
        //    await _repository.GetContext<SqlServerContext>().SaveChangesAsync();
        //}

        public async Task<ProcessEndOfDayResponse> GetEndOfDayResume(long storeId, DateTime initDate, DateTime endDate)
        {
            ProcessEndOfDayResponse result = new ProcessEndOfDayResponse();

            var ends = await _repository.GetEntity<EndOfDay>()
                .Where(x => x.StoreId == storeId && x.SaleDay >= initDate && x.SaleDay <= endDate)
                .ToListAsync();

            if (ends.Count == 0)
            {
                throw new BusinessLogicException("There is no data in the selected date range.");
            }

            var storeName = await _repository.GetEntity<Store>().Where(x => x.Id == storeId).Select(x => x.Name)
                .FirstOrDefaultAsync();

            // result.ElementId = elementId;
            result.StoreName = storeName;
            result.StoreId = storeId;

            result.InitDate = initDate.ToString("MM/dd/yyyy");
            result.EndDate = endDate.ToString("MM/dd/yyyy");

            result.StartingCash = ends.Sum(x => x.StartingCash) / ends.Count;
            result.CashTotalCounted = ends.Sum(x => x.CashTotalCounted);
            result.CashToDeposit = ends.Sum(x => x.CashToDeposit);
            result.TenderedCashTotal = ends.Sum(x => x.TotalCash);
            result.CashOverShort = result.CashToDeposit - result.TenderedCashTotal;

            result.TotalTaxableSales = ends.Sum(x => x.TotalTaxableSales);
            result.SalesTaxCollected = ends.Sum(x => x.SalesTaxCollected);
            result.TotalNotTaxableSales = ends.Sum(x => x.TotalNotTaxableSales);

            result.TotalFeeAndCharges = ends.Sum(x => x.TotalFeeAndCharges);

            result.BottleReturnTotal = ends.Sum(x => x.BottleReturnTotal);
            result.RefundReturn = ends.Sum(x => x.RefundReturn);
            result.RefundReturnOther = ends.Sum(x => x.RefundReturnOther);
            result.PaidOut = ends.Sum(x => x.PaidOut);

            result.DebitCashBack = ends.Sum(x => x.DebitCashBack);
            result.EBTCashBack = ends.Sum(x => x.EBTCashBack);
            result.TotalCash = ends.Sum(x => x.TotalCash);

            result.GiftCardSales = ends.Sum(x => x.GiftCardSales);
            result.TotalCheck = ends.Sum(x => x.TotalCheck);
            result.CreditCardSales = ends.Sum(x => x.CreditCardSales);
            result.SnapEBTSales = ends.Sum(x => x.SnapEBTSales);
            
            result.ManualDiscount = ends.Sum(x => x.ManualDiscount);
            result.UpcReturns = ends.Sum(x => x.UpcReturns);

            result.TotalCashOut =
                result.RefundReturn +
                result.DebitCashBack +
                result.EBTCashBack +
                result.BottleReturnTotal +
                result.PaidOut;

            result.TotalSales =
                result.GiftCardSales +
                result.CreditCardSales +
                result.TotalCash +
                result.TotalCheck; // ends.Sum(x => x.TotalSales);
            return result;
        }

        public async Task<ProcessEndOfDayResponse> ProcessEndOfDay(long storeId, long elementId, EndOfDayHolder holder,
            DateTime initDate,
            DateTime endDate, bool persist = false)
        {
            var store = await _repository.GetEntity<Store>().Where(x => x.Id == storeId).FirstOrDefaultAsync();

            ProcessEndOfDayResponse result = new ProcessEndOfDayResponse();
            var query = _repository.GetEntity<Sale>()
                // .Include(x => x.Employee)
                // .Include(x => x.Device)
                .Include(x => x.Fees)
                .Include(x => x.Taxs)
                .Include(x => x.Products)
                .Include(x => x.Tenders);
            //Get sales for the date range
            var sales = new List<Sale>();
            if (store.DrawerTraking == DrawerTraking.ByCashier)
            {
                var salesTemp = await query
                    .Where(x => x.StoreId == storeId && x.SaleTime >= initDate && x.SaleTime <= endDate &&
                                x.EndOfDayId == null && x.EmployeeId == elementId)
                    .ToListAsync();
                sales.AddRange(salesTemp);
            }
            else
            {
                var salesTemp = await query
                    .Where(x => x.StoreId == storeId && x.SaleTime >= initDate && x.SaleTime <= endDate &&
                                x.EndOfDayId == null && x.DeviceId == elementId)
                    .ToListAsync();
                sales.AddRange(salesTemp);
            }

            if (sales.Count == 0)
            {
                throw new BussinessValidationException("No found sales");
            }

            //Total sales using tender
            // var totalSales =
            //     sales.Where(x =>
            //             x.Tenders.Any(t => t.Amount > 0))
            //         .Sum(x => x.Tenders.Sum(t =>
            //             (t.Name != "Cash" ? t.ApprovedAmount : t.Amount) - t.CashBack));
            
            
            //get taxsable sales using products
            var totalTaxableSales =
                sales.Sum(x => x.Products.Where(t => t.TaxValue > 0).Sum(t => t.TotalPrice/* - t.TaxValue*/));
            //sales.Where(t => t.Tax > 0).Sum(x =>  x.SubTotal );
            //get tax value
            var totalTaxC0llected =
                sales.Sum(x => x.Products.Where(t => t.TaxValue > 0).Sum(t => t.TaxValue));
            //get all cash tendered
            var tenderedCashTotal = sales.Sum(x =>
                x.Tenders.Where(t => t.Name == "Cash" && t.Amount > 0).Sum(t => t.Amount - t.CashBack));

            //var totalNotTaxableSales = sales.Sum(x => x.Products.Where(t => t.TaxValue == 0).Sum(t => t.TotalPrice));
            //Fees
            var totalFeeAndCharges = sales.Sum(x => x.Fees.Sum(t => t.Amount));
            //Calculate bottle return using products
            var bottleReturnTotal =
                sales.Sum(x => x.Products.Where(p => p.Name == "Bottle Refund").Sum(t => t.TotalPrice));

            //var refundReturnCash = sales.Sum(x => x.Tenders.Where(p => p.Name == "Cash" && p.Amount < 0).Sum(t => t.Amount));
            //var refundReturnOthers = sales.Sum(x => x.Tenders.Where(p => p.Name != "Cash" && p.Amount < 0).Sum(t => t.Amount));

            
            
            var refundReturnCash = sales.Where(s => s.Products.Any(x => x.Name == "Return"))
                .Sum(x => x.Tenders.Where(p => p.Name == "Cash").Sum(t => t.Amount));


            var refundReturnOthers = sales.Where(s => s.Products.Any(x => x.Name == "Return"))
                .Sum(x => x.Tenders.Where(p => p.Name != "Cash" && !p.Name.Contains("EBT")).Sum(t => t.Amount));

            var refundReturnSnapEbt = sales.Where(s => s.Products.Any(x => x.Name == "Return"))
                .Sum(x => x.Tenders.Where(p => p.Name.Contains("EBT")).Sum(t => t.Amount));

            
            var upcReturns = sales.Sum(x => x.Products.Where(p => p.SaleProductType == SaleProductType.Return).Sum(t => t.TotalPrice));
            
            
            var manualDiscounts = Math.Abs(sales.Sum(x => x.Products.Where(p => p.SaleProductType == SaleProductType.ManualDiscount).Sum(t => t.Price)));
            
            var totalChecks = sales
                .Sum(x => x.Tenders.Where(p => p.Name == "Check").Sum(t =>  t.Amount));

            //paid out
            var paidOutCash = sales.Where(s => s.Products.Any(x => x.Name == "Paid Out" || x.SaleProductType == SaleProductType.PaidOut))
                .Sum(x => x.Tenders.Where(p => p.Name == "Cash").Sum(t => t.Amount));

            //Debit cashback
            var debitCashBack = sales.Sum(x => x.Tenders.Where(t => t.CardType == "Debit Card").Sum(t => t.CashBack));
            //ebt cashback
            var eBTCashBack = sales.Sum(x => x.Tenders.Where(t => t.Name.Contains("EBT")).Sum(t => t.CashBack));
            //Credit card sales using tender
            var creditCardSales = sales.Sum(x =>
                x.Tenders.Where(t => t.CardType == "Credit Card" && t.Amount > 0).Sum(t => t.ApprovedAmount));
            //snap ebt sales using tender
            var snapEBTSales =
                sales.Sum(x => x.Tenders.Where(t => t.Name == "Snap/EBT").Sum(t => t.ApprovedAmount));
            //Gift card sales using tender
            var giftCardSales = sales.Sum(x =>
                x.Tenders.Where(t => t.Name.Contains("Gift Card") && t.Amount > 0).Sum(t => t.Amount));

            //fill persist object
            EndOfDay eod = new EndOfDay();

            eod.State = true;
            eod.TrackingType = store.DrawerTraking;
            eod.StoreId = storeId;
            eod.ElementId = elementId;
            eod.ElementName = sales[0].Username;

            eod.Sales = sales;
            eod.SalesCount = sales.Count;

            eod.TotalTaxableSales = totalTaxableSales;
            eod.SalesTaxCollected = totalTaxC0llected;
            
            //Sales
            eod.TenderedCashTotal = tenderedCashTotal;
            eod.CreditCardSales = creditCardSales;
            eod.SnapEBTSales = snapEBTSales;
            eod.GiftCardSales = giftCardSales;
            eod.TotalCheck = totalChecks;
            // total all sales
            eod.TotalSales = eod.TenderedCashTotal+ 
                             eod.CreditCardSales + 
                             eod.SnapEBTSales+
                             eod.GiftCardSales+eod.TotalCheck;

            eod.ManualDiscount = manualDiscounts;
            eod.UpcReturns = upcReturns;
            
            eod.TotalFeeAndCharges = totalFeeAndCharges;
            eod.BottleReturnTotal = Math.Abs(bottleReturnTotal);
            eod.RefundReturn = Math.Abs(refundReturnCash);
            eod.RefundReturnEbt = Math.Abs(refundReturnSnapEbt);
            eod.RefundReturnOther = Math.Abs(refundReturnOthers);
            eod.DebitCashBack = debitCashBack;
            eod.EBTCashBack = eBTCashBack;
            eod.PaidOut = Math.Abs(paidOutCash);

            eod.TotalCash =
                eod.TenderedCashTotal -
                (eod.BottleReturnTotal + eod.DebitCashBack + eod.RefundReturn +
                 eod.EBTCashBack +
                 eod.PaidOut); //( Tendered Cash - (Bottle Refund + Return if Cash + Cash Back on Debitand EBT))

            eod.StartingCash = holder.StartingCash;
            eod.Count100 = holder.Count100;
            eod.Count50 = holder.Count50;
            eod.Count20 = holder.Count20;
            eod.Count10 = holder.Count10;
            eod.Count5 = holder.Count5;
            eod.Count1 = holder.Count1;
            eod.Countc100 = holder.Countc100;
            eod.Countc50 = holder.Countc50;
            eod.Countc25 = holder.Countc25;
            eod.Countc10 = holder.Countc10;
            eod.Countc5 = holder.Countc5;
            eod.Countc1 = holder.Countc1;

            //process caounted data

            eod.CashTotalCounted =
                (holder.Count100 * 100) +
                (holder.Count50 * 50) +
                (holder.Count20 * 20) +
                (holder.Count10 * 10) +
                (holder.Count5 * 5) +
                (holder.Count1 + holder.Countc100) +
                (holder.Countc50 * 0.5m) +
                (holder.Countc25 * 0.25m) +
                (holder.Countc10 * 0.10m) +
                (holder.Countc5 * 0.05m) +
                (holder.Countc1 * 0.01m);
            eod.CashToDeposit = eod.CashTotalCounted - eod.StartingCash;
            eod.CashOverShort = eod.CashToDeposit - eod.TotalCash; // here we need add total cash

            //store the data on database
            eod.SaleDay = initDate;

            if (persist)
            {
                await _repository.CreateAsync(eod);
            }

            //fill view object
            result.ElementId = elementId;
            result.StoreName = store.Name;
            result.StoreId = storeId;

            result.ElementName = eod.ElementName;

            result.ManualDiscount = manualDiscounts;
            result.UpcReturns = upcReturns;
            result.TotalCheck = totalChecks;

            result.StartingCash = eod.StartingCash;
            result.CashTotalCounted = eod.CashTotalCounted;
            result.CashToDeposit = eod.CashToDeposit;
            result.TenderedCashTotal = eod.TenderedCashTotal;
            result.CashOverShort = eod.CashOverShort;

            result.TotalTaxableSales = eod.TotalTaxableSales;
            result.SalesTaxCollected = eod.SalesTaxCollected;
            result.TotalNotTaxableSales = eod.TotalNotTaxableSales;
            result.TotalSales = eod.TotalSales;

            result.TotalCashOut =
                eod.RefundReturn +
                eod.DebitCashBack +
                eod.EBTCashBack +
                eod.BottleReturnTotal +
                eod.PaidOut;

            result.TotalFeeAndCharges = eod.TotalFeeAndCharges;

            result.BottleReturnTotal = eod.BottleReturnTotal;
            result.RefundReturn = eod.RefundReturn;
            result.RefundReturnEbt = eod.RefundReturnEbt;
            result.RefundReturnOther = eod.RefundReturnOther;
            result.PaidOut = eod.PaidOut;

            result.DebitCashBack = eod.DebitCashBack;
            result.EBTCashBack = eod.EBTCashBack;
            result.TotalCash = eod.TotalCash;

            result.GiftCardSales = eod.GiftCardSales;
            result.CreditCardSales = eod.CreditCardSales;
            result.SnapEBTSales = eod.SnapEBTSales;

            return result;
        }

        public async Task<bool> ExistSale(string invoice, DateTime saleTme, decimal subTotal)
        {
            return await _repository.GetEntity<Sale>()
                .Where(x => x.Invoice == invoice && x.SaleTime == saleTme && x.SubTotal == subTotal)
                .FirstOrDefaultAsync() != null;
        }

        public async Task<SalesByHourResponse> GetSalesByHour(int begin_hour, int end_hour)
        {
            var stores = await _repository.GetEntity<Store>().ToListAsync();
            SalesByHourResponse result = new SalesByHourResponse();
            foreach (var s in stores)
            {
                var d =  await GetSalesByHourAndStore( s.OpenTime.ToUniversalTime().Hour,  s.ClosedTime.ToUniversalTime().Hour, s.Id, s.TimeZoneId);
                result = result + d;
            }
            // var coralina = await GetSalesByHourAndStore( begin_hour,  end_hour, 4, "Eastern Standard Time");
            // var yavpe = await GetSalesByHourAndStore( begin_hour,  end_hour, 3, "Eastern Standard Time");
            // return coralina + yavpe;
            return result;
        }
        public async Task<SalesByHourResponse> GetSalesByHourAndStore(int begin_hour, int end_hour, long storeId, string timezoneId)
        {//localVersion = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

            /*
            DateTime datetime = DateTime.UtcNow;
            
            int timezoneOffset = -6;//timezone.GetUtcOffset(datetime).Hours;
            try
            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
                timezoneOffset = timezone.GetUtcOffset(datetime).Hours;
            }
            catch
            { }
            */
            DateTime datetime = DateTime.Now;
            
            
            Dictionary<int, decimal> dict = new Dictionary<int, decimal>();
            for (int i = begin_hour; i < end_hour; i++)
            {
                dict.Add(i, 0);
            }
            /*
            var begin = new DateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0, DateTimeKind.Utc);
            //adjust timezone
            begin = begin.AddHours(timezoneOffset);
            
            var currentDayEnd = new DateTime(
                datetime.Year,
                datetime.Month,
                datetime.Day, 23, 59, 59, DateTimeKind.Utc);
            //adjust timezone
            // currentDayEnd = currentDayEnd.AddHours(timezoneOffset);
            */
            var begin = new DateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0);
            var currentDayEnd = new DateTime(datetime.Year, datetime.Month, datetime.Day, 23, 59, 59);

            var salesTemp = await _repository.GetEntity<Sale>()
                .Include(x => x.Tenders)
                .Where(x => x.StoreId == storeId).OrderBy(x=>x.SaleTime).ToListAsync();
            
            var salesToday = await _repository.GetEntity<Sale>()
                .Include(x => x.Tenders)
                .Where(x =>
                    x.StoreId == storeId &&
                    x.SaleTime >= begin &&
                    x.SaleTime <= currentDayEnd /* &&
                    x.SaleTime.Date == DateTime.Today.Date*/)
                .Select(x => new Sale()
                {
                    SaleTime = x.SaleTime,
                    Tenders = x.Tenders.Select(y => new SaleTender()
                    {
                        Name = y.Name,
                        Amount = y.Amount,
                        CashBack = y.CashBack,
                        ApprovedAmount = y.ApprovedAmount
                    }).ToList()
                })
                .ToListAsync();
            
            List<SalesByHourItemResponse> items = new List<SalesByHourItemResponse>();
            foreach (var sale in salesToday)
            {
                /*
                var hourR = sale.SaleTime.Hour - timezoneOffset;
                if (sale.SaleTime.Hour - timezoneOffset > 23)
                {
                    hourR -= 24;
                }

                if (sale.Tenders != null && dict.Keys.Contains(hourR))
                {
                    decimal amount = 0;
                    foreach (var tender in sale.Tenders)//.Where(st => st.Amount > 0))
                    {
                        amount += (tender.Name == "Cash") ? (tender.Amount - tender.CashBack) : tender.ApprovedAmount;
                    }
                    dict[hourR] += amount;   
                }
                */
                var hourR = sale.SaleTime.Hour;
                
                if (sale.Tenders != null && dict.Keys.Contains(hourR))
                {
                    decimal amount = 0;
                    foreach (var tender in sale.Tenders)//.Where(st => st.Amount > 0))
                    {
                        amount += (tender.Name == "Cash" || tender.Name == "Check") ? (tender.Amount - tender.CashBack) : tender.ApprovedAmount;
                    }
                    dict[hourR] += amount;   
                }
            }

            foreach (var d in dict)
            {
                items.Add(new SalesByHourItemResponse()
                {
                    InitialHour = d.Key,
                    HourInterval = d.Key + " - " + (d.Key + 1),
                    Amount = d.Value
                });
            }

            var result = new SalesByHourResponse();
            result.Items = items;
            result.TotalAmount = items.Sum(x => x.Amount);
            return result;
        }

        //si este metodo sigue demorando implementar un cache de los valors en base de datos para solo calcular el dia de la semana actual los anteriores tomarlos de la bd
        // y el domincgo se borran todos los existentes y se empieza de nuevo, hacer esto o usar algo en Redis o mongo para almacenar si es mas facil pero hacer algo asi
        // para que no sea tan engorroso estos calculos
        //Igual en caso de tiendas mas grandes se pueden cachear los calculos de la priemra hora de la manana para solo tener que calcular el resto y asi mejorar la eficiencia 
        // de este endpoint
        public async Task<SalesByHourResponse> GetSalesByWeek(long storeId)
        {//localVersion = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            /*
            DateTime datetime = DateTime.UtcNow.StartOfWeek();
            
            var lastSunday = new DateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0, DateTimeKind.Utc);
            
            var currentDayBegin = lastSunday;
            var currentDayEnd = new DateTime(
                currentDayBegin.Year,
                currentDayBegin.Month,
                currentDayBegin.Day, 23, 59, 59, DateTimeKind.Utc);
            */
            DateTime datetime = DateTime.Now.StartOfWeek();
            
            var lastSunday = new DateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0 );
            
            var currentDayBegin = lastSunday;
            var currentDayEnd = new DateTime(
                currentDayBegin.Year,
                currentDayBegin.Month,
                currentDayBegin.Day, 23, 59, 59);
            // Dictionary<int, decimal[]> dict = new Dictionary<int, decimal[]>();
            int finalCount = 0;
            decimal totalWeekAmount = 0m;

            List<SalesByHourItemResponse> items = new List<SalesByHourItemResponse>();
            for (int i = 0; i < 7; i++)
            {
                var salesTenderCurrentDay = await _repository.GetEntity<SaleTender>()
                    .Include(x => x.Sale)
                    .Where(x =>
                        x.Sale.SaleTime >= currentDayBegin &&
                        x.Sale.SaleTime <= currentDayEnd &&
                        x.Sale.StoreId == storeId
                        //x.Amount > 0
                        )
                    .Select(x => new
                    {
                        Name = x.Name,
                        Amount = x.Amount,
                        CashBack = x.CashBack,
                        ApprovedAmount = x.ApprovedAmount,
                        SaleId = x.SaleId
                    })
                    .ToListAsync();

                var amountCash = salesTenderCurrentDay.Where(st => st.Name == "Cash" || st.Name == "Check")
                    .Sum(st => (st.Amount - st.CashBack));
                var amountCards = salesTenderCurrentDay.Where(st => st.Name != "Cash" && st.Name != "Check")
                    .Sum(st => (st.ApprovedAmount));

                var customerCount = salesTenderCurrentDay.Select(x => x.SaleId).Distinct().Count();
                finalCount += customerCount;
                var average = (customerCount != 0) ? (amountCash + amountCards) / customerCount : 0;
                totalWeekAmount += amountCash + amountCards;
                // dict.Add(i, new decimal[] {amountCash + amountCards, customerCount, average});
                items.Add(new SalesByHourItemResponse()
                {
                    DayOfWeek = Enum.GetName(typeof(DayOfWeek), i),
                    Amount = amountCash + amountCards,
                    CustomerCount = customerCount,
                    Average = average
                });
                currentDayBegin = currentDayBegin.AddDays(1);
                currentDayEnd = currentDayEnd.AddDays(1);
            }


            // foreach (var d in dict)
            // {
            //     items.Add(new SalesByHourItemResponse()
            //     {
            //         DayOfWeek = Enum.GetName(typeof(DayOfWeek), d.Key),
            //         Amount = d.Value[0],
            //         CustomerCount = (int) d.Value[1],
            //         Average = d.Value[2]
            //     });
            // }
            var result = new SalesByHourResponse();
            result.Items = items;
            result.TotalAmount = items.Sum(x => x.Amount);

            /*
            DateTime dateNow = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(dateNow.Year, dateNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            */
            DateTime dateNow = DateTime.Now;
            var firstDayOfMonth = new DateTime(dateNow.Year, dateNow.Month, 1, 0, 0, 0);
            
            var salesM = await _repository.GetEntity<Sale>()
                .Include(x => x.Tenders)
                .Where(x =>
                    x.SaleTime >= firstDayOfMonth &&
                    x.SaleTime <= dateNow && 
                    x.StoreId == storeId )
                .Select(x => new
                {
                    Tenders = x.Tenders.Select<SaleTender, SaleTender>(y => new SaleTender()
                    {
                        Name = y.Name,
                        Amount = y.Amount,
                        CashBack = y.CashBack,
                        ApprovedAmount = y.ApprovedAmount
                    })
                })
                .ToListAsync();

            var amountCash1 = salesM
                .Sum(s =>
                    s.Tenders.Where(t =>
                            t.Name == "Cash" || t.Name == "Check"//&&
                            //t.Amount > 0
                            )
                        .Sum(st => (st.Amount - st.CashBack)));
            var amountCards1 = salesM
                .Sum(s =>
                    s.Tenders.Where(t =>
                            t.Name != "Cash" && t.Name != "Check" //&&
                            //t.Amount > 0
                                )
                        .Sum(st => st.ApprovedAmount));
            result.AllMonth = amountCash1 + amountCards1;
            result.WeekCount = finalCount;
            result.Average = (finalCount != 0) ? totalWeekAmount / finalCount : 0;
            return result;
        }

        public async Task<List<SalesSearchResponseDetailDto>> GetPosFilteredSale(Device device, SalesSearchRequestDto filter)
        {
            DateTime begindate = DateTime.UtcNow;
            if (filter.SaleDay.HasValue)
            {
                begindate = filter.SaleDay.Value.ToUniversalTime().Date;
            }
            return  await _repository.GetEntity<Sale>()
                // .Include(x => x.Device)
                //.Include(x => x.Tax)
                //.Include(x => x.Discounts)
                .Include(x => x.Tenders)
                //.Include(x => x.Fees)
                .Where(x => 
                    x.StoreId == device.StoreId 
                    )
                .WhereIf(filter.SaleDay.HasValue, x => x.SaleTime >= begindate && x.SaleTime <= new DateTime( begindate.Year,
                    begindate.Month,
                    begindate.Day, 23, 59, 59, DateTimeKind.Utc))
                .WhereIf(filter.DeviceId.HasValue, x => x.DeviceId == filter.DeviceId.Value)
                
               .WhereIf(filter.TotalAmount.HasValue, x => x.Total == filter.TotalAmount.Value)
                //.WhereIf(filter.CardNumber.HasValue, x => x.Tenders.Any(t => t.Pan.EndsWith(filter.CardNumber)))
                //.WhereIf(filter.Invoice.HasValue, x => x.Invoice == filter.Invoice)
                .Select(x => new SalesSearchResponseDetailDto()
                {
                    SaleId = x.Id,
                    TotalAmount = x.Total,
                    Register = x.Username,
                    SaleDay = x.SaleTime
                })
                .ToListAsync();
        }

        // public async Task FixSaleTenderDataCards()
        // {
        //     //corregir los numeros de invoice de las ventas
        //     // var sales = await _repository.GetEntity<Sale>()
        //     //     .Where(x => x.Invoice.StartsWith("38" + x.EmployeeId))
        //     //     .ToListAsync();
        //     //
        //     // foreach (var sale in sales)
        //     // {
        //     //     sale.Invoice = "38" + sale.EmployeeId + sale.LocalId.ToString("000000");
        //     //     _logger.LogInformation($"new invoice {sale.Invoice}");
        //     // }
        //     // await _repository.UpdateRangeAsync(sales);
        //     
        //     //extrer del rawresponse la data necesaria para el autorization code
        //     var tenders = await _repository.GetEntity<SaleTender>()
        //         .Where(x => x.RawResponse != null)
        //         .ToListAsync();
        //
        //     foreach (var sale in tenders)
        //     {
        //         var response = System.Text.Json.JsonSerializer.Deserialize<Response>(sale.RawResponse);
        //         sale.ResultCode = response.data.transactionResponse;
        //         sale.AuthCode = response.data.authorizationCode;
        //     }
        //     await _repository.UpdateRangeAsync(tenders);
        //     
        //     
        //     //obtener todas las ventas de cards con RawResponse <> null
        // }

        public async Task FixDuplicationSales()
        {
            var now = DateTime.UtcNow;
            var beginDay = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var currentDayEnd = new DateTime(
                beginDay.Year,
                beginDay.Month,
                beginDay.Day, 23, 59, 59, DateTimeKind.Utc);
            ;

            var sales = await _repository.GetEntity<Sale>()
                .Where(x =>
                    x.SaleTime >= beginDay &&
                    x.SaleTime <= currentDayEnd)
                .Select(x => new Sale
                {
                    SaleTime = x.SaleTime,
                    Invoice = x.Invoice,
                    SubTotal = x.SubTotal,
                    LocalId = x.LocalId,
                    Id = x.Id
                })
                .ToListAsync();

            _logger.LogInformation($"Begin");
            var holder = sales.ToList();
            var processeds = new List<long>();
            foreach (var s in holder)
            {
                processeds.Add(s.Id);
                if (sales.Where(x =>
                        x.SaleTime >= beginDay &&
                        x.SaleTime <= currentDayEnd &&
                        x.Invoice == s.Invoice &&
                        x.SubTotal == s.SubTotal &&
                        x.LocalId == s.LocalId &&
                        !processeds.Any(p => p == x.Id)).ToList().Count() == 0) continue;
                _logger.LogInformation($"Procesing invoice {s.Invoice}");
                var toremove = await _repository.GetEntity<Sale>()
                    .Where(x =>
                        x.SaleTime >= beginDay &&
                        x.SaleTime <= currentDayEnd &&
                        x.Invoice == s.Invoice &&
                        x.SubTotal == s.SubTotal &&
                        x.LocalId == s.LocalId &&
                        !processeds.Any(p => p == x.Id))
                    .Select(x => new Sale
                    {
                        Id = x.Id
                    })
                    .ToListAsync();

                if (toremove.Count > 0)
                    await _repository.DeleteRangeAsync(toremove);
            }

            _logger.LogInformation($"End");
        }
    }

    // public class Response
    // {
    //     public string RawResponse { get; set; }
    //     public string error { get; set; }
    //     public ResponseData data { get; set; }
    //     public ResponseHeader header { get; set; }
    // }
    //
    //  public class ResponseData
    // {
    //     public string invoice { get; set; }
    //     public string amount { get; set; }
    //     public string ebtAmount { get; set; }
    //     /// <summary>
    //     /// if debit and is more than 0 then this is cashback
    //     /// </summary>
    //     public string cashAmount { get; set; }
    //     public string authorizationCode { get; set; }
    //     /// <summary>
    //     /// 0 debit 1 credit 3 maybe debit too
    //     /// </summary>
    //     public string cardType { get; set; }
    //     /// <summary>
    //     /// PaymentTypeDebit ||  PaymentTypeCredit
    //     /// </summary>
    //     public string paymentType { get; set; }
    //     /// <summary>
    //     /// /name for card owner
    //     /// </summary>
    //     public string cardholderName { get; set; }
    //     /// <summary>
    //     /// ???
    //     /// </summary>
    //     public string exp { get; set; }
    //     /// <summary>
    //     /// Transaction id i thing this is the number for return or refund this transaction
    //     /// </summary>
    //     public string transactionId { get; set; }
    //     //Response code
    //     public string transactionResponse { get; set; }
    //     /// <summary>
    //     /// masked card number
    //     /// </summary>
    //     public string pan { get; set; }
    //     /// <summary>
    //     /// Card marx amex visa ect
    //     /// </summary>
    //     public string issuerName { get; set; }
    //
    //     public string hostResponseText { get; set; }
    //
    //     #region Commtest
    //     public string terminalProfile { get; set; }
    //     public string terminalSoftwareVersion { get; set; }
    //     public string terminalOSVersion { get; set; }
    //     public string operationalStatus { get; set; }
    //
    //     #endregion
    //
    //     #region BatchClose
    //
    //     public string acquirerName { get; set; }
    //     public string batch { get; set; }
    //     public string batchRecordCount { get; set; }
    //     public string hostTotalsAmount1 { get; set; }
    //     public string hostTotalsAmount5 { get; set; }
    //     public string hostTotalsCount1 { get; set; }
    //     public string hostTotalsCount5 { get; set; }
    //     public string merchantName { get; set; }
    //     public string terminalNumber { get; set; }
    //     #endregion
    // }
    // public class ResponseHeader
    // {
    //     public string indicator { get; set; }
    //     /// <summary>
    //     /// userCancel | approved
    //     /// </summary>
    //     public string response { get; set; }
    //     public string transaction { get; set; }
    //     public string version { get; set; } = "2";
    // }
}