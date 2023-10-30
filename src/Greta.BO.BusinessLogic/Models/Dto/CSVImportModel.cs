using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities.Enum;
using Microsoft.AspNetCore.Http;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class CSVImportModel
    {
        [Required] public long MappingId { get; set; }

        [Required] public IFormFile CSVFile { get; set; }

        public List<string> ModelHeader { get; set; }
        public List<string> CsvHeader { get; set; }

        [Required] public char Separator { get; set; }

        [Required] public ModelImport ModelImport { get; set; }

        public List<long> StoresId { get; set; }
        public List<StoreModel> Stores { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}