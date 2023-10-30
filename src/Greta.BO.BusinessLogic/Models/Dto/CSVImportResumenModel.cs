using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class CSVImportResumenModel
    {
        [Required] public int TotalRow { get; set; }

        [Required] public int InsertedRowCount { get; set; }

        [Required] public int FailedRowCount { get; set; }

        [Required] public List<string> Errors { get; set; }
    }
}