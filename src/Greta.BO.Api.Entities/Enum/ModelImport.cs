using System.ComponentModel.DataAnnotations;

namespace Greta.BO.Api.Entities.Enum
{
    public enum ModelImport
    {
        PRODUCT,
        [Display(Name = "SCALE PRODUCT")] SCALE_PRODUCT,
        DEPARTMENT,
        CATEGORY,
        [Display(Name = "SCALE CATEGORY")] SCALE_CATEGORY,
        [Display(Name = "Weighed PRODUCT")] W_PRODUCT,
        FAMILY
    }
}