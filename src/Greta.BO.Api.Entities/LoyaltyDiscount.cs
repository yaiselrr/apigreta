using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class LoyaltyDiscount : BaseEntityLong, IFullSyncronizable
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Maximum { get; set; }
        public long StoreId { get; set; }
        public Store Store { get; set; }
    }
}