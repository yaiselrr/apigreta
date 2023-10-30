using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
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
    public static class BOUserCreate
    {
        public record Command(BOUserModel entity) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"add_edit_user")
            };
        }

        public class Validator : AbstractValidator<Command>
        {
            private readonly IBOUserService _service;
            private readonly IProfilesService profilesService;

            public Validator(IBOUserService service, IProfilesService profilesService)
            {
                _service = service;
                this.profilesService = profilesService;

                RuleFor(x => x.entity.BOProfileId)
                    .MustAsync(CheckBOProfileId)
                    .WithMessage($"{nameof(BOUserModel.BOProfileId)} isn't Id from BO Application.");

                RuleFor(x => x.entity.POSProfileId)
                    .MustAsync(CheckPOSProfileId)
                    .WithMessage($"{nameof(BOUserModel.POSProfileId)} isn't Id from POS Application.");

                RuleFor(x => x.entity)
                    .Must(CheckPOS).WithMessage("User must have a profile.");
            }

            private async Task<bool> CheckBOProfileId(long? id, CancellationToken cancellationToken)
            {
                if (id.Value != -1)
                {
                    return true;
                }

                if (!await profilesService.ExistWithThisApplication(id.Value, 1)) return false;
                return true;
            }

            private async Task<bool> CheckPOSProfileId(long? id, CancellationToken cancellationToken)
            {
                if (id.Value != -1)
                {
                    return true;
                }

                if (!await profilesService.ExistWithThisApplication(id.Value, 2)) return false;
                return true;
            }

            private bool CheckPOS(BOUserModel entity)
            {
                if (entity.POSProfileId == -1 && entity.BOProfileId == -1) return false;
                return true;
            }
        }


        public class Handler : IRequestHandler<Command, Response>
        {
            protected readonly IAuthenticateUser<string> _authenticateUser;
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IBOUserService _service;
            private readonly IRequestClient<CreateUserRequestContract> _clientCreate;
            private readonly IRequestClient<DeleteUserRequestContract> _clientDelete;
            private readonly MainOption options;

            public Handler(
                IOptions<MainOption> options,
                ILogger<Handler> logger,
                IBOUserService service,
                IMapper mapper,
                IRequestClient<CreateUserRequestContract> client,
                IAuthenticateUser<string> authenticateUser,
                IRequestClient<DeleteUserRequestContract> clientDelete)
            {
                this.options = options.Value;
                _logger = logger;
                _service = service;
                _mapper = mapper;
                _authenticateUser = authenticateUser;
                _clientCreate = client;
                _clientDelete = clientDelete;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Api.Entities.BOUser>(request.entity);

                var data =
                    await _clientCreate.GetResponse<CreateUserResponseContract, FailResponseContract>
                    (new
                    {
                        __Header_application = options.CompanyCode,
                        __Header_user = _authenticateUser.UserId,
                        request.entity.FirstName,
                        request.entity.LastName,
                        request.entity.UserName,
                        request.entity.Email,
                        request.entity.PhoneNumber,
                        request.entity.CanCreateUsers,
                        UserCreatorId = _authenticateUser.UserId,
                        request.entity.State
                    });

                //entity.UserId  .Message.UserId
                if (data.Is(out Response<CreateUserResponseContract> responseA))
                {
                    entity.UserId = responseA.Message.UserId;
                }
                else if (data.Is(out Response<FailResponseContract> responseB))
                {
                    _logger.LogError("Page parameter (currentPage or pageSize) out of bounds.");
                    throw new BusinessLogicException(((List<string>) responseB.Message.ErrorMessages)[0]);
                }

                try
                {
                    //create user into Bo
                    var userBO = await _service.Post(entity);

                    //clear pass and set Id
                    // request.entity.Password = null;
                    request.entity.Id = userBO.Id;

                    _logger.LogInformation($"Create BOUser {userBO.Id} for user {userBO.UserCreatorId}");
                    return new Response {Data = request.entity};
                }
                catch (Exception e)
                {
                    //delete user on identity if create user in BO fail
                    _logger.LogInformation("Create BOUser fail");
                    await _clientDelete.Create(new {entity.UserId}).GetResponse<Greta.Sdk.MassTransit.Contracts.BooleanResponseContract>();
                    throw new Exception(e.Message, e.InnerException);
                }
            }
        }

        public record Response : CQRSResponse<BOUserModel>;
    }
}