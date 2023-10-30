using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Options;
using Greta.BO.BusinessLogic.Service;
using Greta.Identity.Api.EventContracts;
using Greta.Identity.Api.EventContracts.BO.User;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.MassTransit.Contracts;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.BusinessLogic.Handlers.Command.BOUser
{
    public class BOUserDelete
    {
        public record Command(long Id) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"delete_user")
            };
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            protected readonly IAuthenticateUser<string> _authenticateUser;
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IBOUserService _service;
            private readonly IRequestClient<DeleteUserRequestContract> _client;
            private readonly MainOption options;

            public Handler(
                ILogger<Handler> logger,
                IBOUserService service,
                IMapper mapper,
                IOptions<MainOption> options,
                IAuthenticateUser<string> authenticateUser,
                IRequestClient<DeleteUserRequestContract> client)
            {
                this.options = options.Value;
                _authenticateUser = authenticateUser;
                _logger = logger;
                _service = service;
                _mapper = mapper;
                _client = client;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var userBO = await _service.GetById(request.Id);
                var result = await _client.GetResponse<BooleanResponseContract, FailResponseContract>(new
                {
                    __Header_application = options.CompanyCode,
                    __Header_user = _authenticateUser.UserId,
                    userBO.UserId
                });

                if (result.Is(out Response<BooleanResponseContract> responseA))
                {
                    try
                    {
                        await _service.Delete(request.Id);
                    }
                    catch (Exception e)
                    {
                        //delete user Bo fail
                        _logger.LogInformation($"BOUser {request.Id} delete fail.");
                        throw new Exception(e.Message, e.InnerException);
                    }
                }
                else if (result.Is(out Response<FailResponseContract> responseB))
                {
                    //update user identity fail
                    _logger.LogInformation($"BOUser {request.Id} update fail.");
                    throw new BusinessLogicException(((List<string>) responseB.Message.ErrorMessages)[0]);
                }

                _logger.LogInformation($"Entity with id {request.Id} Deleted successfully.");
                return new Response {Data = true};
            }
        }

        public record Response : CQRSResponse<bool>;
    }
}