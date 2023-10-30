using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Greta.BO.Api.Responses
{
    [ExcludeFromCodeCoverage]
    public class ApiErrorResponse
    {
        public ApiErrorResponse(string msg) : this(new List<string> {msg})
        {
        }

        public ApiErrorResponse(IEnumerable<string> errorMessages)
        {
            Errors = errorMessages;
        }

        public IEnumerable<string> Errors { get; set; }

        public string Detail { get; set; }
    }
}