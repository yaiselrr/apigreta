using System;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto.LoyaltyDiscountDto
{
    public class LoyaltyDiscountCreateModel: /*IDtoLong<string>,*/ IMapFrom<LoyaltyDiscount>
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Maximum { get; set; }
        public long StoreId { get; set; }


        public long Id { get; set; }
        //public bool State { get; set; }
        //public string UserCreatorId { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
    }    
}