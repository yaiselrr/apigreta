using System;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class ScaleCategoryModel : IDtoLong<string>, IMapFrom<ScaleCategory>
    {
        [Required]
        [StringLength(64, ErrorMessage = "The {0} field not is valid")]
        public string Name { get; set; }

        [Required] public long DepartmentId { get; set; }

        [Required] public int CategoryId { get; set; }

        public DepartmentModel Department { get; set; }
        public long? ParentId { get; set; }

        public ScaleCategoryModel Parent { get; set; }

        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long Id { get; set; }
    }
}