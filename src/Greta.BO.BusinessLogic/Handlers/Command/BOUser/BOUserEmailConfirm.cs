using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Abstractions;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Options;
using Greta.Identity.Api.EventContracts;
using Greta.Identity.Api.EventContracts.BO.User;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.MassTransit.Contracts;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.BusinessLogic.Handlers.Command.BOUser
{
    public class BOUserEmailConfirm
    {
        public record Command(long Id) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"add_edit_user")
            };
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            protected readonly IAuthenticateUser<string> _authenticateUser;
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IBOUserRepository _repository;
            private readonly IRequestClient<UserEmailConfirmRequestContract> _client;
            private readonly MainOption options;

            public Handler(
                ILogger<Handler> logger,
                IBOUserRepository repository,
                IMapper mapper,
                IOptions<MainOption> options,
                IAuthenticateUser<string> authenticateUser,
                IRequestClient<UserEmailConfirmRequestContract> client)
            {
                this.options = options.Value;
                _authenticateUser = authenticateUser;
                _logger = logger;
                _repository = repository;
                _mapper = mapper;
                _client = client;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var userId = (await _repository.GetEntity<Api.Entities.BOUser>()
                    .FirstOrDefaultAsync(u => u.Id == request.Id))?.UserId;
                if (userId == null)
                {
                    _logger.LogInformation($"BOUser {request.Id} not found.");
                    return new Response {Data = false};
                }

                //update user into Identity
                var result = await _client.GetResponse<BooleanResponseContract, FailResponseContract>(new
                {
                    __Header_application = options.CompanyCode,
                    __Header_user = _authenticateUser.UserId,
                    Id = userId
                });
                if (result.Is(out Response<FailResponseContract> response))
                {
                    //update user identity fail
                    _logger.LogInformation($"User identity {request.Id} not found.");
                    throw new BusinessLogicException(((List<string>) response.Message.ErrorMessages)[0]);
                }

                _logger.LogInformation($"BOUser {request.Id} email confirmed successfully.");
                return new Response {Data = true};
            }
        }

        public record Response : CQRSResponse<bool>;
    }
}