using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Options;
using Greta.BO.BusinessLogic.Service;
using Greta.Identity.Api.EventContracts.BO.User;
using Greta.Sdk.EFCore.Middleware;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.BusinessLogic.Handlers.Queries.User
{
    public static class BOUserGetAll
    {
        public record Query : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"view_{nameof(User).ToLower()}")
            };
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            protected readonly IAuthenticateUser<string> _authenticateUser;
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IBOUserService _service;
            private readonly IRequestClient<GetAllUserRequestContract> _client;
            private readonly MainOption options;

            public Handler(ILogger<Handler> logger, IAuthenticateUser<string> authenticateUser,
                IOptions<MainOption> options, IBOUserService service, IMapper mapper,
                IRequestClient<GetAllUserRequestContract> client)
            {
                this.options = options.Value;
                _authenticateUser = authenticateUser;
                _logger = logger;
                _service = service;
                _mapper = mapper;
                _client = client;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                //get users list from identity
                var usersIdentity = (await _client.GetResponse<GetAllUserResponseContract>(new
                {
                    __Header_application = options.CompanyCode,
                    __Header_user = _authenticateUser.UserId
                })).Message.Users;
                //get users list from Bo
                var usersBo = await _service.Get();
                //create usersDto list to return
                var usersDto = new List<BOUserModel>();
                usersBo.ForEach(user =>
                    usersDto.Add(new BOUserModel(user, usersIdentity.FirstOrDefault(u => u.Id == user.UserId))));

                return new Response {Data = usersDto};
            }
        }

        public record Response : CQRSResponse<List<BOUserModel>>;
    }
}