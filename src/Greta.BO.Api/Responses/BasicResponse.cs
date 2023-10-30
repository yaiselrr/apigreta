using System.Diagnostics.CodeAnalysis;

namespace Greta.BO.Api.Responses
{
    [ExcludeFromCodeCoverage]
    public class BasicResponse
    {
        public BasicResponse(string msg)
        {
            Message = msg;
        }

        public int Status { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
    }
}