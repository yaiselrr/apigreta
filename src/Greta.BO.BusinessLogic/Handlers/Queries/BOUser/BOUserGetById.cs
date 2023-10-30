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

namespace Greta.BO.BusinessLogic.Handlers.Queries.BOUser
{
    public static class BOUserGetById
    {
        public record Query(long Id) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new() {
                new PermissionRequirement.Requirement($"view_{nameof(User).ToLower()}")
            };
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            protected readonly IAuthenticateUser<string> _authenticateUser;
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IBOUserService _service;
            private readonly IRequestClient<UserGetInfoRequestContract> _client;
            private readonly MainOption options;


            public Handler(
                ILogger<Handler> logger,
                IBOUserService service,
                IMapper mapper,
                IOptions<MainOption> options,
                IAuthenticateUser<string> authenticateUser,
                IRequestClient<UserGetInfoRequestContract> client)
            {
                this.options = options.Value;
                _authenticateUser = authenticateUser;
                _logger = logger;
                _service = service;
                _mapper = mapper;
                _client = client;
                this.options = options.Value;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                //get user info from BO
                var userBo = await _service.Get(request.Id);
                if (userBo != null)
                {
                    //get user info from identity
                    var result = await _client.GetResponse<UserGetInfoResponseContract, FailResponseContract>(
                        new
                        {
                            __Header_application = options.CompanyCode,
                            __Header_user = _authenticateUser.UserId,
                            userBo.UserId
                        });
                    //update userBO uf userIdentity was updeted successfuly
                    if (result.Is(out Response<UserGetInfoResponseContract> responseA))
                    {
                        var user = new BOUserModel(userBo, responseA.Message);
                        return new Response {Data = user};
                    }

                    if (result.Is(out Response<FailResponseContract> responseB))
                    {
                        //update user identity fail
                         _logger.LogInformation($"User identity {userBo.UserId} no found.");
                        throw new BusinessLogicException(((List<string>) responseB.Message.ErrorMessages)[0]);
                    }
                }

                _logger.LogInformation($"BOUser {request.Id} not found.");
                return null;
            }
        }

        public record Response : CQRSResponse<BOUserModel>;
    }
}