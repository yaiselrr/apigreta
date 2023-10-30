using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using System.Collections.Generic;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class CSVExportModel
    {
        public List<string> Columns { get; set; }

    }
}