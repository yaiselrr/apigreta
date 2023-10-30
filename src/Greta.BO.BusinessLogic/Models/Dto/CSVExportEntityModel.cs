using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class CSVExportEntityModel<T> where T: new()
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public List<string> ModelHeader { get; set; }

        [Required]
        public List<T> CsvHeader { get; set; }

        [Required]
        public ModelImport ModelImport { get; set; }

        public long Id { get; set; }

        public bool State { get; set; }

        public string UserCreatorId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
