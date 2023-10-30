using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class AddBreakPack
    {
        [Required]
        public List<SetParentProductModel> Elements { get; set; }
    }
    
    public class SetParentProductModel
    {
        [Required]
        public long Parent { get; set; }
        [Required]
        public long Child { get; set; }
        [Required]
        public decimal Count { get; set; }
    }
}