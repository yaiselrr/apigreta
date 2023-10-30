using System;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class FamilyModel : IDtoLong<string>, IMapFrom<Family>
    {
        [Required]
        [StringLength(30, ErrorMessage = "The {0} field not is valid")]
        public string Name { get; set; }

        public long Id { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    
    public class FamilyExportModel : IMapFrom<Family>
    {
        public string Name { get; set; }
        public long Id { get; set; }
    }
}