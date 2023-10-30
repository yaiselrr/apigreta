using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Greta.BO.Api.Dto;
using Greta.BO.Api.Responses;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Greta.BO.Api.Filters
{
    [ExcludeFromCodeCoverage]
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            try
            {
                this._logger.LogError(context.Exception, context.Exception.Message);
                if (context.Exception.GetType() == typeof(BusinessLogicException))
                {
                    var exception = context.Exception as BusinessLogicException;
                    // var json = new ApiErrorResponse(exception.Message);
                    var json = new CQRSResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = new List<string> {exception.Message}
                    };

                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    context.Result = new JsonResult(json);
                }
                else if (context.Exception.GetType() == typeof(BussinessValidationException))
                {
                    var exception = context.Exception as BussinessValidationException;
                    // var json = new ApiErrorResponse(exception.Errors);
                    var json = new CQRSResponse<string>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = exception.Errors
                    };

                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    context.Result = new JsonResult(json);
                }
                // else if (context.Exception.GetType() == typeof(Microsoft.EntityFrameworkCore.DbUpdateException))
                // {
                //     var exception = context.Exception as Microsoft.EntityFrameworkCore.DbUpdateException;

                //     // var errorMessages = exception.InnerException. Entries
                //     //                     .SelectMany(x => x.State ValidationErrors)
                //     //                     .Select(x => x.ErrorMessage);

                //     // var json = new ApiErrorResponse(exception.Errors);
                //     var json = new CQRSResponse<string>()
                //     {
                //         StatusCode = HttpStatusCode.BadRequest,
                //         Errors = new List<string>() { "An error occurred while updating the entries." }//exception.Errors
                //     };

                //     context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //     context.Result = new JsonResult(json);
                // }
                else
                {
                    //if (!context.HttpContext.Response.HasStarted)
                    //{
                    // Unhandled errors
                    //#if !DEBUG
                    //                var msg = "An unhandled error occurred.";                
                    //                string stack = null;
                    //#else
                    var msg = context.Exception.GetBaseException().Message;
                    var stack = context.Exception.StackTrace;
                    //#endif

                    var json = new CQRSResponse<string>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = new List<string> {msg},
                        Data = stack
                    };

                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                    // handle logging here

                    // always return a JSON result
                    context.Result = new JsonResult(json);
                }
                //}

                context.ExceptionHandled = true;
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}