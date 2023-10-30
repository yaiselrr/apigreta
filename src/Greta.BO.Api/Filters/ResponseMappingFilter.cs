using System.Diagnostics.CodeAnalysis;
using System.Net;
using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Greta.BO.Api.Filters
{
    [ExcludeFromCodeCoverage]
    public class ResponseMappingFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult && objectResult.Value is CQRSResponse cqrsResponse &&
                cqrsResponse.StatusCode != HttpStatusCode.OK)
                context.Result = new ObjectResult(new {cqrsResponse.Errors})
                    {StatusCode = (int) cqrsResponse.StatusCode};
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}