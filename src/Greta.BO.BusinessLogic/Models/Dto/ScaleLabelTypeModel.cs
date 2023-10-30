using System;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class ScaleLabelTypeModel : IDtoLong<string>, IMapFrom<ScaleLabelType>
    {
        [Required]
        [StringLength(64, ErrorMessage = "The {0} field not is valid")]
        public string Name { get; set; }

        /// <summary>
        ///     This value if ScaleType is GretaLabel then this is 500+
        ///     if ScaleType is External then this value is 1 to 250
        ///     Not user for shelftags
        /// </summary>
        /// <value></value>
        [Required]
        public int LabelId { get; set; }

        [Required] public ScaleType ScaleType { get; set; }

        /// <summary>
        ///     This is only used for Shelf tags ans GretaLabels, not for externals
        /// </summary>
        /// <value></value>
        public string Design { get; set; }
        
        // public ScaleLabelDesignModel ScaleLabelDesign{ get; set; }

        public long Id { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    
    public class ScaleLabelTypeListModel : IDtoLong<string>, IMapFrom<ScaleLabelType>
    {
        public string Name { get; set; }
        [Required]
        public int LabelId { get; set; }

        [Required] public ScaleType ScaleType { get; set; }

        public long Id { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    
    // public class ScaleLabelDesignModel : IDtoLong<string>, IMapFrom<ScaleLabelDesign>
    // {
    //
    //     public ScaleType ScaleType { get; set; }
    //
    //     public string Design { get; set; }
    //
    //     public long Id { get; set; }
    //
    //     public bool State { get; set; }
    //     public string UserCreatorId { get; set; }
    //     public DateTime CreatedAt { get; set; }
    //     public DateTime UpdatedAt { get; set; }
    // }
}