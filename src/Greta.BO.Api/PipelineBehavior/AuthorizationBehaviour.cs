using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Greta.BO.Api.PipelineBehavior
{
    [ExcludeFromCodeCoverage]
    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IAuthorizable
        // where TResponse : CQRSResponse, new()
    {
        private readonly ILogger<AuthorizationBehaviour<TRequest, TResponse>> _logger;
        private readonly IServiceProvider _serviceProvider;

        public AuthorizationBehaviour(ILogger<AuthorizationBehaviour<TRequest, TResponse>> logger,
            IServiceProvider serviceProvider)
        {
            this._logger = logger;
            this._serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = request.GetType();
            _logger.LogInformation("{Request} is authorizable", requestName);

            // Loop through each requirement for the request and authorise
            foreach (var requirement in request.Requirements)
            {
                var handlerType = typeof(IRequirementHandler<>).MakeGenericType(requirement.GetType());
                var handler = _serviceProvider.GetRequiredService(handlerType);
                var methodInfo = handler.GetType().GetMethod(nameof(IRequirementHandler<IRequirement>.Handle));
                var result = await (Task<AuthorizationResult>)methodInfo.Invoke(handler, new[] { requirement });
                if (result.IsAuthorized)
                {
                    _logger.LogInformation("{Requirement} has been authorized for {Request}",
                        requirement.GetType().Name, requestName);
                    continue;
                }

                _logger.LogWarning("{Requirement} FAILED for {Request}", requirement.GetType().Name, requestName);
                // return new TResponse { StatusCode = HttpStatusCode.Unauthorized };
                throw new Greta.BO.BusinessLogic.Exceptions.BusinessLogicException(result.Message);
            }

            _logger.LogInformation("{Request} authorization was successful", requestName);
            return await next();
        }
    }
}