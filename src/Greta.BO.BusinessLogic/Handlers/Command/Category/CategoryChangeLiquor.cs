using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Category;

/// <summary>
/// Change isLiquor of the entity
/// </summary>
/// <param name="Id">Entity Id</param>
/// <param name="IsLiquorActive">Is Liquor Active</param>
public record CategoryChangeLiquorCommand(long Id, bool IsLiquorActive) : IRequest<CategoryChangeLiquorResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Category).ToLower()}")
    };
}


/// <inheritdoc />
public class CategoryChangeLiquorHandler : IRequestHandler<CategoryChangeLiquorCommand, CategoryChangeLiquorResponse>
{
    private readonly ILogger _logger;
    private readonly ICategoryService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public CategoryChangeLiquorHandler(ILogger<CategoryChangeLiquorHandler> logger, ICategoryService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CategoryChangeLiquorResponse> Handle(CategoryChangeLiquorCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.ChangeLiquor(request.Id, request.IsLiquorActive);
        _logger.LogInformation("Entity with id {RequestId} isLiquor change to {RequestLiquor}", request.Id, request.IsLiquorActive);
        return new CategoryChangeLiquorResponse { Data = result };
    }
}

/// <inheritdoc />
public record CategoryChangeLiquorResponse : CQRSResponse<bool>;