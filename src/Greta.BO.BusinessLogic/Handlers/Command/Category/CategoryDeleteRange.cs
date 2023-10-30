using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Entities.Events.Internal.Categories;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.Wix.Handlers.Commands.Categories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Category;

/// <summary>
/// Delete entities by entity ids
/// </summary>
/// <param name="Ids"></param>
public record CategoryDeleteRangeCommand(List<long> Ids) : IRequest<CategoryDeleteRangeResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Category).ToLower()}")
    };
}

/// <inheritdoc />
public class CategoryDeleteRangeHandler : IRequestHandler<CategoryDeleteRangeCommand, CategoryDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly ICategoryService _service;
    private readonly IMediator _mediator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public CategoryDeleteRangeHandler(
        ILogger<CategoryDeleteRangeHandler> logger,
        ICategoryService service,
        IMediator mediator,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CategoryDeleteRangeResponse> Handle(CategoryDeleteRangeCommand request,
        CancellationToken cancellationToken)
    {
        var idScanRemove = new List<long>();
        foreach (var id in request.Ids)
        {
            if (!await _service.CanDeleted(id))
            {
                idScanRemove.Add(id);
            }
        }

        await _mediator.Publish(new CategoryDeleted(request.Ids.Except(idScanRemove).ToList(), idScanRemove), cancellationToken);

        var result = await _service.DeleteRange(request.Ids.Except(idScanRemove).ToList());
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new CategoryDeleteRangeResponse { Data = result };
    }
}

/// <inheritdoc />
public record CategoryDeleteRangeResponse : CQRSResponse<bool>;