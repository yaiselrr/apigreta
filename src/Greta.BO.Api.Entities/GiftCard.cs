using System;
using System.Collections.Generic;
using Greta.Sdk.EFCore.Interfaces;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities
{
    public class GiftCard: BaseEntityLong
    {
        public long SaleId { get; set; }
        public string Number { get; set; }
        
        public decimal Amount { get; set; }
        
        public decimal Balance { get; set; }
        
        public DateTime LastUsed { get; set; }

        public DateTime DateSold { get; set; }

        public long EmployeeId { get; set; }
        
        public string EmployeeName { get; set; }

        public long? DeviceId { get; set; }
        
        public Device Device { get; set; }
        
        public long StoreId { get; set; }
        
        public Store Store { get; set; }
        
        public virtual List<GiftCardTransaction> Transactions { get; set; }
    }

    public class GiftCardTransaction: BaseEntityLong
    {
        public long SaleId { get; set; }
        public decimal Amount { get; set; }

        public DateTime DateUse { get; set; }

        public long EmployeeId { get; set; }

        public string EmployeeName { get; set; }
        
        public long Device { get; set; }
        
        public long StoreId { get; set; }

        public GiftCardRequestStatus Status { get; set; }

        public long GiftCardId { get; set; }

        public GiftCard GiftCard { get; set; }
    }
}