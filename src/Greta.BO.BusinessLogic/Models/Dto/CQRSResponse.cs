using System.Collections.Generic;
using System.Net;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public record CQRSResponse
    {
        public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;

        // public string ErrorMessage { get; init; }
        public IEnumerable<string> Errors { get; init; }
    }

    public record CQRSResponse<TData> : CQRSResponse
    {
        public TData Data { get; init; }
    }
}