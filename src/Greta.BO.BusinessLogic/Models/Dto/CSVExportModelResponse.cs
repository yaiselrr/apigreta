using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class CSVExportModelResponse : IDtoLong<string>
    {
        [Required] public string Name { get; set; }

        [Required] public List<string> ModelHeader { get; set; }

        [Required] public List<string> CsvHeader { get; set; }

        [Required] public ModelImport ModelImport { get; set; }

        public long Id { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}