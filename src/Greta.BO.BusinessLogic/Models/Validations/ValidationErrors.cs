using System.Collections.Generic;

namespace Greta.BO.BusinessLogic.Models.Validations
{
    public struct ValidationErrors
    {
        public IEnumerable<ErrorDescription> Errors { get; set; }
    }

    public struct ErrorDescription
    {
        public string Field { get; set; }
        public string Description { get; set; }
    }
}