using System;
using System.Collections.Generic;
using System.Linq;
using Greta.BO.Api.Entities;
using Greta.Sdk.Printing.Models;

namespace Greta.BO.BusinessLogic.Extensions
{
    public static class ProductSaleCollectionExtensions
    {
        public static RecipeModel ToReceiptModel(this Sale collection, Device device, bool desgloseTaxes = false, bool allowzero = false)
        {
            return new RecipeModel(
                1,
                collection.Invoice,
                device.Name + "(Reprint)",
                DateTime.Now.ToString("M/d/yyyy HH:ss tt"),
                collection.SubTotal.ToMoneyOrEmpty(allowzero),//SubTotal
                "$0.00", //Ebt
                collection.Discount.ToMoneyOrEmpty(allowzero),//Discount
                collection.ServiceFee.ToMoneyOrEmpty(),//ServiceFee
                collection.CashDiscount.ToMoneyOrEmpty(allowzero),//CashDiscount
                collection.TenderCash.ToMoneyOrEmpty(allowzero),//TenderCash
                collection.Tax.ToMoneyOrEmpty(allowzero),//Tax
                collection.Total.ToMoneyOrEmpty(allowzero),//Total
                collection.ClearCashDiscountTotal.ToMoneyOrEmpty(allowzero),//ClearCashDiscountTotal
                Math.Abs(collection.ChangeDue).ToMoneyOrEmpty(allowzero),//ChangeDue
                 false,//Transparency
                false,
                 new RecipeStoreModel(
                    device.Store.Id.ToString(),
                    device.Store.Name,
                    device.Store.Slogan,
                    device.Store.Address,
                    //config.StateOfCountry,
                    //config.City,
                    device.Store.Zip,
                    device.Store.Phone,
                    ""
                 ),
                 collection.Products.Select(x => x.ToReceiptProduct(false, allowzero)).ToList(),
                 desgloseTaxes
                    ? collection.Taxs.Select(x => new Greta.Sdk.Printing.Models.RecipeTax(x.Name, x.Amount.ToMoneyOrEmpty())).ToList()
                    : new List<Greta.Sdk.Printing.Models.RecipeTax>(),
                 collection.Discounts == null ? new List<RecipeDiscount>() : collection.Discounts.Select(x => new RecipeDiscount(x.Name, x.Amount.ToMoneyOrEmpty())).ToList(),
                  collection.Tenders.Select(x => new Greta.Sdk.Printing.Models.RecipeTender(x.CardType == null ? x.Name : x.CardType, x.Amount.ToMoneyOrEmpty(), x.Pan, x.IssuerName)).ToList(),
                 collection.Fees.Select(x => new RecipeFee(x.Name, x.Amount.ToMoneyOrEmpty())).ToList()
                );

        }

        public static RecipeProduct ToReceiptProduct(this SaleProduct item, bool toInternalNetwork, bool allowzero = false)
        {
            return new RecipeProduct(
               item.UPC,//string Upc,
                item.Name,//string Name,
                item.Price.ToMoneyOrEmpty(allowzero),//string Price,
                toInternalNetwork ? item.QTY.ToString() : ((((double)Math.Abs(item.QTY % 1)) <= (Double.Epsilon * 100)) ? item.QTY.ToString() : $"{item.QTY} lb @ {item.Price.ToMoneyOrEmpty(allowzero)}"), 
                item.TotalPrice.ToMoneyOrEmpty(allowzero), //string Total,
                item.Subtitle //string Subtitle
                );
        }

        public static string ToMoneyOrEmpty(this decimal value, bool allowzero = false)
        {
            if(allowzero)
                return value.ToString("C2");
            else
                return value == 0 ? "" : value.ToString("C2");
        }

        public static string ToMoney(this decimal value)
        {
            return value.ToString("C2");
        }
        public static string ToFixPercentValue(this decimal value, int type = 0)
        {
            switch (type)
            {
                case 0:
                    return $"${value}";
                case 1:
                    return $"{value}%";
                default:
                    return "";
            }
        }
    }
}