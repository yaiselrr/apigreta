using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.BOUser
{
    public record BoUserResetPinCommand(BOUserResetPinModel Entity) : IRequest<BoUserResetPinResponse>, IAuthorizable
    {
        public List<IRequirement> Requirements => new()
        {
            new PermissionRequirement.Requirement($"add_edit_user")
        };
    }

    public record BoUserResetPinResponse : CQRSResponse<bool>;
    
    public class BoUserResetPinHandler : IRequestHandler<BoUserResetPinCommand, BoUserResetPinResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IBOUserService _service;

        public BoUserResetPinHandler(
            ILogger<BoUserResetPinHandler> logger,
            IBOUserService service,
            IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        public async Task<BoUserResetPinResponse> Handle(BoUserResetPinCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Entity.Pin != request.Entity.PinConfirm)
            {
                throw new BussinessValidationException("Pins must match");
            }

            //get user
            var user = await _service.Get(request.Entity.Id);
            if (user == null)
            {
                _logger.LogError("User with id  {UserId} not found", request.Entity.Id);
                throw new BussinessValidationException("User not found");
            }
            //Check if pin exist
            var newPin = BCrypt.Net.BCrypt.HashPassword(request.Entity.Pin);
            
            var existUser = await _service.GetByPin(newPin);

            if (existUser != null)
            {
                _logger.LogError("Pin used not valid on the system", request.Entity.Id);
                throw new BussinessValidationException("Pin used not valid on the system");
            }
            user.Pin = newPin;

            var result = await _service.PutBase(request.Entity.Id, user);
            _logger.LogInformation("User {Username} change pin successfully", user.UserName);
            return new BoUserResetPinResponse() { Data = result };
        }
    }
}