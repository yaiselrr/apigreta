using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Family;

/// <summary>
/// Delete entity by entity id
/// </summary>
/// <param name="Id"></param>
public record FamilyDeleteCommand(long Id) : IRequest<FamilyDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Family).ToLower()}")
    };
}

/// <inheritdoc />
public class FamilyDeleteHandler : IRequestHandler<FamilyDeleteCommand, FamilyDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly IFamilyService _service;

    /// <inheritdoc />
    public class Validator : AbstractValidator<FamilyDeleteCommand>
    {
        private readonly IFamilyService _service;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public Validator(IFamilyService service)
        {
            _service = service;

            RuleFor(x => x.Id)
                .MustAsync(CanDeleted)
                .WithMessage($"This family cannot be deleted because it is associated with another element");
        }

        private async Task<bool> CanDeleted(long id, CancellationToken cancellationToken)
        {
            return await _service.CanDeleted(id);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public FamilyDeleteHandler(
        ILogger<FamilyDeleteHandler> logger,
        IFamilyService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<FamilyDeleteResponse> Handle(FamilyDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!await _service.CanDeleted(request.Id))
        {
            throw new BusinessLogicException(
                "This family cannot be deleted because it is associated with another element");
        }

        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new FamilyDeleteResponse { Data = result };
    }
}

/// <inheritdoc />
public record FamilyDeleteResponse : CQRSResponse<bool>;