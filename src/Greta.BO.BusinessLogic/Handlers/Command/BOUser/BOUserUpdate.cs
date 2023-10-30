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
    public class BOUserUpdate
    {
        public record Command(long Id, BOUserModel entity) : IRequest<Response>, IAuthorizable
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
            private readonly IRequestClient<UpdateUserRequestContract> _client;
            private readonly MainOption options;

            public Handler(
                ILogger<Handler> logger,
                IBOUserService service,
                IMapper mapper,
                IOptions<MainOption> options,
                IAuthenticateUser<string> authenticateUser,
                IRequestClient<UpdateUserRequestContract> client)
            {
                this.options = options.Value;
                _authenticateUser = authenticateUser;
                _logger = logger;
                _service = service;
                _mapper = mapper;
                _client = client;
                _authenticateUser = authenticateUser;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Api.Entities.BOUser>(request.entity);
                entity.Id = request.Id;
                //set userId in entity request
                request.entity.UserId = (await _service.GetById(request.Id))?.UserId;

                //update user into Identity
                var result = await _client.GetResponse<BooleanResponseContract, FailResponseContract>(new
                {
                    __Header_application = options.CompanyCode,
                    __Header_user = _authenticateUser.UserId,
                    request.entity.UserId,
                    request.entity.FirstName,
                    request.entity.LastName,
                    request.entity.CanCreateUsers,
                    request.entity.UserName,
                    request.entity.Email,
                    request.entity.PhoneNumber,
                    request.entity.State,
                    request.entity.TwoFactorEnabled
                });
                //update userBO uf userIdentity was updeted successfuly
                if (result.Is(out Response<BooleanResponseContract> responseA))
                {
                    try
                    {
                        await _service.Put(request.Id, entity);
                    }
                    catch (Exception e)
                    {
                        //update user Bo fail
                        _logger.LogInformation($"BOUser {request.Id} update fail.");
                        throw new Exception(e.Message, e.InnerException);
                    }
                }
                else if (result.Is(out Response<FailResponseContract> responseB))
                {
                    //update user identity fail
                    _logger.LogInformation($"BOUser {request.Id} update fail.");
                    throw new BusinessLogicException(((List<string>) responseB.Message.ErrorMessages)[0]);
                }

                _logger.LogInformation($"BOUser {request.Id} update successfully.");
                return new Response {Data = true};
            }
        }

        public record Response : CQRSResponse<bool>;
    }
}