using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation;
namespace Greta.BO.BusinessLogic.Handlers.Command.OnlineStore;

/// <summary>
/// Delete entity by entity id
/// </summary>
/// <param name="Id"></param>
public record OnlineStoreDeleteCommand(long Id) : IRequest<OnlineStoreDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Store).ToLower()}")
    };
}

/// <inheritdoc />
public class OnlineStoreDeleteHandler : IRequestHandler<OnlineStoreDeleteCommand, OnlineStoreDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly IOnlineStoreService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public OnlineStoreDeleteHandler(
        ILogger<OnlineStoreDeleteHandler> logger,
        IOnlineStoreService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public class Validator : AbstractValidator<OnlineStoreDeleteCommand>
    {
        private readonly IOnlineStoreService _service;

        /// <inheritdoc />
        public Validator(IOnlineStoreService service)
        {
            _service = service;

            RuleFor(x => x.Id)
                .MustAsync(CanDeleted)
                .WithMessage($"This online store cannot be deleted because it is associated with another element.");
        }

        private async Task<bool> CanDeleted(long id, CancellationToken cancellationToken)
        {
            return await _service.CanDeleted(id);
        }
    }

    /// <inheritdoc />
    public async Task<OnlineStoreDeleteResponse> Handle(OnlineStoreDeleteCommand request, CancellationToken cancellationToken)
    {
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new OnlineStoreDeleteResponse { Data = result };
    }
}

/// <inheritdoc />
public record OnlineStoreDeleteResponse : CQRSResponse<bool>;