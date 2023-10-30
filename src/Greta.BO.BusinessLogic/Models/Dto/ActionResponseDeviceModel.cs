using System.Diagnostics.CodeAnalysis;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    /// <summary>
    /// Describe a Device Response
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ActionResponseDeviceModel
    {
        public string Message { get; set; }
        public bool State { get; set; } = true;
    }
}