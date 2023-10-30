using System;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Identity.Api.EventContracts.BO.User;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class BOUserResetPinModel //: IMapFrom<BOUser>, IDtoLong<string>
    {
        public string Pin { get; set; }
        public string PinConfirm { get; set; }

        public long Id { get; set; }
        
        // public bool State { get; set; }
        // public string UserCreatorId { get; set; }
        // public DateTime CreatedAt { get; set; }
        // public DateTime UpdatedAt { get; set; }
    }
}