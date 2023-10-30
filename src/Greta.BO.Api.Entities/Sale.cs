
using System;
using System.Collections.Generic;

namespace Greta.BO.Api.Entities
{
    public class Sale:BaseEntityLong
    {
        public decimal ClearCashDiscountTotal { get; set; }
        public long LocalId { get; set; }
        public string Invoice { get; set; }

        public long StoreId { get; set; }
        public long DeviceId { get; set; }
        public long EmployeeId { get; set; }
        public string Username { get; set; }
        public long? CustomerId { get; set; }
        public decimal CustomerDiscountAmount { get; set; }
        public int CustomerDiscountPointsUsed { get; set; }

        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal CashDiscount { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal TenderCash { get; set; }
        public decimal Total { get; set; }
        public decimal ChangeDue { get; set; }


        public bool UseSpecialTaxes { get; set; }
        public DateTime SaleTime { get; set; }
        public long? EndOfDayId { get; set; }
        public EndOfDay EndOfDay { get; set; }
        public List<SaleTender> Tenders { get; set; }
        public List<SaleProduct> Products { get; set; }
        public List<SaleTax> Taxs { get; set; }
        public List<SaleDiscount> Discounts { get; set; }
        public List<SaleFee> Fees { get; set; }
        public List<SaleProductRemoved> SaleProductRemoveds { get; set; }
        
        
        #region DriveLicense
        public bool IsOver40 { get; set; }
        public string DriveLicenseRaw { get; set; }
        public string DriveLicenseFirstName { get; set; }
        public string DriveLicenseMiddleName { get; set; }
        public string DriveLicenseFamilyName { get; set; }
        public string DriverLicenseNumber { get; set; }
        public string DriveLicenseDocumentDiscriminator { get; set; }
        public DateTime DriveLicenseBirthday { get; set; }
        public DateTime DriveLicenseExpirationday { get; set; }
        #endregion
    }

 

  
}
