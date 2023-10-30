using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.Wix.Handlers.Commands.Categories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Category;

/// <summary>
/// Delete entity by entity id
/// </summary>
/// <param name="Id"></param>
public record CategoryDeleteCommand(long Id) : IRequest<CategoryDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Category).ToLower()}")
    };
}

/// <inheritdoc />
public class CategoryDeleteHandler : IRequestHandler<CategoryDeleteCommand, CategoryDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly IOnlineStoreService _storeService;
    private readonly IMediator _mediator;
    private readonly ICategoryService _service;

    /// <inheritdoc />
    public class Validator : AbstractValidator<CategoryDeleteCommand>
    {
        private readonly ICategoryService _service;

        /// <inheritdoc />
        public Validator(ICategoryService service)
        {
            _service = service;

            RuleFor(x => x.Id)
                .MustAsync(CanDeleted)
                .WithMessage("This category cannot be deleted because it is associated with another element");
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
    public CategoryDeleteHandler(
        ILogger<CategoryDeleteHandler> logger,
        IOnlineStoreService storeService,
        IMediator mediator,
        ICategoryService service)
    {
        _logger = logger;
        _storeService = storeService;
        _mediator = mediator;
        _service = service;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="BusinessLogicException"></exception>
    public async Task<CategoryDeleteResponse> Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!await _service.CanDeleted(request.Id))
        {
            throw new BusinessLogicException("This category cannot be deleted because it is associated with another element");
        }
        var onlineReferences = await _service.GetWithOnlineStores(request.Id);
        foreach (var cat in onlineReferences.OnlineCategories)
        {
            //remove category from online store
            await _mediator.Send(new DeleteCommand(cat.OnlineStore.RefreshToken, cat.OnlineCategoryId), cancellationToken);
        }
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new CategoryDeleteResponse { Data = result };
    }
}

/// <inheritdoc />
public record CategoryDeleteResponse : CQRSResponse<bool>;