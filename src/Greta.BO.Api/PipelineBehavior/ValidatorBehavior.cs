using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Greta.BO.Api.Dto;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.Api.PipelineBehavior
{
    [ExcludeFromCodeCoverage]
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        //where TResponse : CQRSResponse, new()
    {
        private readonly ILogger<ValidatorBehavior<TRequest, TResponse>> _logger;
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidatorBehavior(ILogger<ValidatorBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public ValidatorBehavior(
            ILogger<ValidatorBehavior<TRequest, TResponse>> logger,
            IEnumerable<IValidator<TRequest>> validators)
        {
            _logger = logger;
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // pre actions
            var requestName = request.GetType();
            if (_validators == null)
            {
                _logger.LogInformation("{Request} does not have a validation handler configured", requestName);
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);
             var failures = _validators
                .Select(x => ( x.ValidateAsync(context, cancellationToken)).Result)
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .ToList();

            if (failures.Any())
            {
                //Change this for OneOf system
                var errorMessages = failures.Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning("Validation failed for {Request}. Errors: {Error}", requestName, errorMessages);
                //throw new BussinessValidationException(errorMessages);
                // return new TResponse {StatusCode = HttpStatusCode.BadRequest, Errors = errorMessages};
                throw new Greta.BO.BusinessLogic.Exceptions.BusinessLogicException(errorMessages.Count > 0 ? errorMessages[0]: "Unknown error");
            }

            _logger.LogInformation("Validation successful for {Request}", requestName);
            // action
            return await next();
            // post action
        }
    }
}