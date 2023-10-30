using System;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class ScaleLabelDefinitionModel : IDtoLong<string>, IMapFrom<ScaleLabelDefinition>
    {
        [Required] public long ScaleProductId { get; set; }

        [Required] public long ScaleLabelType1Id { get; set; }

        public long ScaleLabelType2Id { get; set; }

        [Required] public long ScaleBrandId { get; set; }

        public virtual ScaleProductModel ScaleProduct { get; set; }
        public virtual ScaleLabelTypeModel ScaleLabelType1 { get; set; }
        public virtual ScaleLabelTypeModel ScaleLabelType2 { get; set; }

        public long Id { get; set; }

        /// <summary>
        ///     No used on this entity
        /// </summary>
        /// <value></value>
        public bool State { get; set; }

        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}