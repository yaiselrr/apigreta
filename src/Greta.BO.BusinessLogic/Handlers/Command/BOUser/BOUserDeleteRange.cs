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
    public class BOUserDeleteRange
    {
        public record Command(List<long> Ids) : IRequest<Response>, IAuthorizable
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
            private readonly IRequestClient<DeleteUsersRangeRequestContract> _client;
            private readonly MainOption options;

            public Handler(
                ILogger<Handler> logger,
                IBOUserService service,
                IMapper mapper,
                IOptions<MainOption> options,
                IAuthenticateUser<string> authenticateUser,
                IRequestClient<DeleteUsersRangeRequestContract> client)
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
                //get usersBo
                var usersBO = new List<Api.Entities.BOUser>();
                request.Ids.ForEach(id => usersBO.Add(_service.GetById(id).Result));
                //delete users from identity
                var result = (await _client.Create(new
                {
                    __Header_application = options.CompanyCode,
                    __Header_user = _authenticateUser.UserId,
                    UsersId = usersBO.Select(u => u.UserId).ToList()
                }).GetResponse<Greta.Sdk.MassTransit.Contracts.BooleanResponseContract>()).Message;

                if (result.Status)
                    result.Status = await _service.DeleteRange(usersBO.Select(u => u.Id).ToList());

                _logger.LogInformation($"Entity with ids {request.Ids} Deleted successfully.");
                return new Response {Data = result.Status};
            }
        }

        public record Response : CQRSResponse<bool>;
    }
}