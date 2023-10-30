#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CsvHelper;
using Greta.BO.BusinessLogic.Handlers.DataHandlers;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Greta.BO.BusinessLogic.Service;
using System.Data;
using System.Linq;
using Greta.BO.Api.Abstractions;
using Greta.BO.BusinessLogic.TypeConverters;
using Microsoft.EntityFrameworkCore;
using Greta.BO.BusinessLogic.Handlers.DataHandlers.Validations;
using Greta.BO.BusinessLogic.Handlers.DataHandlers.Validations.Extensions;

namespace Greta.BO.BusinessLogic.Handlers.DataHandlers.Importers
{
    public class ProductImport : BaseImport<Product>
    {
        public ProductImport(ILogger<ProductImport> logger, IServiceProvider provider, 
            ILogger<Introspector<Product>> inLogger, INotifier notifier)
            : base(logger, provider, inLogger, notifier)
        {
        }

        public override async Task Process(Dictionary<string, string> mapping, CsvReader csv, List<long> storesIds)
        {
            var productRepository = Provider.GetRequiredService<IProductRepository>();  
            var productService = Provider.GetRequiredService<IProductService>();
            var departmentService = Provider.GetRequiredService<IDepartmentService>();
            var catService = Provider.GetRequiredService<ICategoryService>();
            var storeProductService = Provider.GetRequiredService<IStoreProductService>();
            _currentRow = 0;
            _totalRows = 0;
            _insertedRows = 0;
            _updatedRows = 0;
            _failedRows = 0;
            var validations = new ValidationPipeline<Product>(_notifier)
                .Add<ProductValidationName>()
                .Add<ProductValidationCategoryId>()
                .Add<ProductValidationDepartmentId>()
                .Add<ProductValidationUPC>();
            var products = Mapper(mapping, csv, storesIds, (hm) =>
            {
                if (hm.MessageLevel == MessageLevel.Information)
                    NotifyUpdate(HandlerMessage.From(BuildMessage(), hm));
                else
                {
                    NotifyError(HandlerMessage.From(BuildMessage(), hm, true));
                }
            });
            _totalRows = products.Count;
            // Notify("Initializing");            
            var categoriesCache = new Dictionary<long, Category>();
            var departmentsCache = new Dictionary<long, long>();
            for(var i = 0; i < products.Count; i++)
            {
                _currentRow++;
                var currentProduct = products[i];
                try
                {
                    var validation = await validations.Execute(currentProduct, BuildMessage(), mapping);
                    if (!validation.IsValid)
                    {
                        _failedRows++;
                        _errors.Add($"line: {_currentRow} - {((HandlerMessage)validation.Msg!).Message}");
                        NotifyError(BuildMessage());
                    }
                    else
                    {
                        var product = await productService.GetProductByUPC(currentProduct.UPC);
                        if(product == null)
                        {
                            await productRepository.TransactionAsync(async context =>
                            {
                                long department = departmentsCache.GetOrAddFromCache(currentProduct.DepartmentId,
                                    async (id) => await departmentService.GetByDepartmentId((int)id), 
                                    (hm) =>
                                    {
                                        _failedRows++;
                                        Notify($"line{_currentRow}: {hm.ErrorMessage}", true);
                                        //
                                        // var msg = HandlerMessage.From(BuildMessage(), hm, true);
                                        // _failedRows++;
                                        // _errors.Add($"line: {_currentRow} - {msg.ErrorMessage}");
                                        // NotifyError(msg);
                                    });
                                if(department > 0)
                                {
                                    var category = categoriesCache.GetOrAddFromCache(currentProduct.CategoryId,
                                        async (id) => await catService.GetByCategoryId((int)id),
                                        (hm) =>
                                        {
                                            _failedRows++;
                                            Notify($"line{_currentRow}: {hm.ErrorMessage}", true);
                                            //var msg = HandlerMessage.From(BuildMessage(), hm, true);
                                            // _failedRows++;
                                            // _errors.Add($"line: {_currentRow} - {msg.ErrorMessage}");
                                            // NotifyError(msg);
                                        });
                                    if(category != null)
                                    {
                                        currentProduct.DepartmentId = department;
                                        currentProduct.PopulateIfNotMapped(mapping, category);

                                        var stores = currentProduct.StoreProducts.ToList();
                                        currentProduct.StoreProducts.Clear();

                                        var np = await productService.CreateProduct(currentProduct);
                                        foreach(var store in stores)
                                        {
                                            store.ProductId = np.Id;
                                            await storeProductService.PostImport(store, np);
                                        }
                                        _insertedRows++;
                                        Notify();
                                        return true;
                                    }
                                }
                                return false;
                            }, IsolationLevel.RepeatableRead);
                        }
                        else
                        {
                            currentProduct.Id = product.Id;
                            currentProduct.UserCreatorId = product.UserCreatorId;
                            currentProduct.StoreProducts.ForEach(e =>
                            {
                                var storeProduct = product.StoreProducts.FirstOrDefault(r => r.StoreId == e.StoreId);
                                e.Id = storeProduct == null ? 0 : storeProduct.Id;
                                e.ProductId = product.Id;
                                e.UserCreatorId = storeProduct?.UserCreatorId;
                            });

                            await productRepository.TransactionAsync(async context =>
                            {
                                var department = departmentsCache.GetOrAddFromCache(currentProduct.DepartmentId,
                                    async (id) => await departmentService.GetByDepartmentId((int)id),
                                    (hm) =>
                                    {
                                        _failedRows++;
                                        Notify($"line{_currentRow}: {hm.ErrorMessage}", true);
                                        // NotifyError(HandlerMessage.From(BuildMessage(), hm, true));
                                    });
                                if(department > 0)
                                {
                                    var category =
                                        categoriesCache.GetOrAddFromCache<Category>(currentProduct.CategoryId,
                                            async (id) => await catService.GetByCategoryId((int)id),
                                            (hm) =>
                                            {
                                                _failedRows++;
                                                Notify($"line{_currentRow}: {hm.ErrorMessage}", true);
                                                //NotifyError(HandlerMessage.From(BuildMessage(), hm, true));
                                            });
                                    if(category != null)
                                    {
                                        currentProduct.DepartmentId = department;
                                        currentProduct.PopulateIfNotMapped(mapping, category);

                                        var np = await productService.UpdateProduct(product.Id,
                                            currentProduct);

                                        _updatedRows++;
                                        Notify();
                                        return true;
                                    }
                                }
                                return false;
                            }, IsolationLevel.RepeatableRead);
                        }
                    }
                }
                catch(Exception error)
                {
                    _failedRows++;
                    var msgEr = BuildMessage();
                    if (error.InnerException != null &&
                        error.InnerException.Message.Contains("duplicate"))
                    {
                        if (error.InnerException.Message.Contains("_Name"))
                            msgEr.ErrorMessage = $"Name value must be unique. {products[i].Name}";
                        else if (error.InnerException.Message.Contains("_UPC"))
                            msgEr.ErrorMessage = $"UPC value must be unique. {products[i].UPC}";
                    }
                    else if(error.Message.Contains("conflicted with the FOREIGN KEY constraint") || error.InnerException != null && error.InnerException.Message.Contains("conflicted with the FOREIGN KEY constraint"))
                    {
                        msgEr.ErrorMessage = $"Occurred an conflicted with a FOREIGN KEY.";
                    }
                    else
                    {
                        msgEr.ErrorMessage =  $"{error.Message}{error.InnerException?.Message}";
                    }

                    currentProduct = null;
                    if (error is DbUpdateException ex)
                        foreach (var entry in ex.Entries)
                            // Do some logic or fix
                            // or just detach
                            entry.State = EntityState.Detached;
                    _errors.Add($"line: {_currentRow} - {msgEr.ErrorMessage}");
                    NotifyError(msgEr);

                }
                Notify();
            }
            var msg = BuildMessage();
            msg.Stage = Stage.Completed;
            Notify(msg);
        }

        public List<Product> Mapper(Dictionary<string, string> mapping, CsvReader csv, List<long> storesId,
            Action<HandlerMessage>? notify = null)
        {
            var products = new List<Product>();
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                var product = new Product();
                product.State = true;
                var propsProduct = product.GetBaseProperties();
                foreach (var p in propsProduct)
                {
                    if (product.GetType().GetProperties().Any(x => x.Name == p))
                    {
                        if (product.GetType().GetProperty(p).PropertyType.IsAssignableFrom(typeof(bool)))
                        {
                            var defV = p == "PosVisible" ? true : false;
                            var value = mapping.GetCsValue<bool, BooleanConverter>(csv, p, defV);
                            product.GetType().GetProperty(p)?.SetValue(product, value, null);
                        }
                        else
                        {
                            var val = mapping.GetCsValue(csv, product.GetType().GetProperty(p)?.PropertyType, p, null);
                            product.GetType().GetProperty(p)?.SetValue(product, val, null);
                        }
                    }
                }

                //adding store products to product
                storesId.ForEach(id =>
                {
                    var cost = !mapping.ContainsValue(nameof(StoreProduct.Cost))
                        ? 0
                        : csv.GetField<decimal>(mapping.FirstOrDefault(x => x.Value == nameof(StoreProduct.Cost))
                            .Key);
                    var grossProfit = !mapping.ContainsValue(nameof(StoreProduct.GrossProfit))
                        ? 0
                        : csv.GetField<decimal>(mapping
                            .FirstOrDefault(x => x.Value == nameof(StoreProduct.GrossProfit)).Key);
                    var price = !mapping.ContainsValue(nameof(StoreProduct.Price))
                        ? 0
                        : csv.GetField<decimal>(mapping.FirstOrDefault(x => x.Value == nameof(StoreProduct.Price))
                            .Key);
                    var qtyHand = !mapping.ContainsValue(nameof(StoreProduct.QtyHand))
                        ? 0
                        : csv.GetField<decimal>(mapping.FirstOrDefault(x => x.Value == nameof(StoreProduct.QtyHand))
                            .Key);

                    if (product.StoreProducts == null) product.StoreProducts = new();
                    product.StoreProducts.Add(new StoreProduct
                    {
                        //Calcular el grostprofit si no viene en el csv
                        StoreId = id,
                        // BinLocation = !mapping.ContainsValue(nameof(StoreProduct.BinLocation))
                        //     ? null
                        //     : csv.GetField<string>(mapping
                        //         .FirstOrDefault(x => x.Value == nameof(StoreProduct.BinLocation)).Key),
                        Cost = cost,
                        GrossProfit = grossProfit,
                        Price = price,
                        QtyHand = qtyHand
                        //ProductId = !mapping.ContainsValue(nameof(StoreProduct.ProductId)) ? 0 : csv.GetField<long>(mapping.FirstOrDefault(x => x.Value == nameof(StoreProduct.ProductId)).Key),
                    });
                });

                products.Add(product);
            }

            return products;

        }
    }
}
