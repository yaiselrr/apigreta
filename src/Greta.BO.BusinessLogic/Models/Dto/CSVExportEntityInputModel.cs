#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities.Enum;
using Microsoft.AspNetCore.Http;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class CSVExportEntityInputModel
    {
        [Required]
        public string Model { get; set; }

        public List<string>? Columns { get; set; } = null;
        
        public long? StoreIds { get; set; } = null;
    }
}
