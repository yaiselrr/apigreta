using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Family;

/// <summary>
/// Delete entity by entity id and product Id
/// </summary>
/// <param name="Id"></param>
/// <param name="ProductId"></param>
public record FamilyDeleteProductCommand(long Id, long ProductId) : IRequest<FamilyDeleteProductResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Family).ToLower()}")
    };
}

/// <inheritdoc />
public class FamilyDeleteProductHandler : IRequestHandler<FamilyDeleteProductCommand, FamilyDeleteProductResponse>
{
    private readonly ILogger _logger;
    private readonly IFamilyService _service;

    /// <inheritdoc />
    public class FamilyDeleteProductValidator : AbstractValidator<FamilyDeleteProductCommand>
    {
        private readonly IFamilyService _service;

        /// <inheritdoc />
        public FamilyDeleteProductValidator(IFamilyService service)
        {
            _service = service;

            RuleFor(x => x.Id)
                .MustAsync(CanDeleted)
                .WithMessage($"This product family cannot be deleted because it is associated with another element");
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
    public FamilyDeleteProductHandler(ILogger<FamilyDeleteProductHandler> logger, IFamilyService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<FamilyDeleteProductResponse> Handle(FamilyDeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.DeleteProduct(request.Id, request.ProductId);
        _logger.LogInformation("Entity with id {RequestProductId} Deleted successfully", request.ProductId);
        return new FamilyDeleteProductResponse { Data = result };
    }
}

/// <inheritdoc />
public record FamilyDeleteProductResponse : CQRSResponse<bool>;