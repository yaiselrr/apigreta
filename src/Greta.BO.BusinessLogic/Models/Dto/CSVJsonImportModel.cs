using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class CSVJsonImportModel
    {
        [Required] public string Name { get; set; }

        [Required] public IFormFile JSONFile { get; set; }
    }
}