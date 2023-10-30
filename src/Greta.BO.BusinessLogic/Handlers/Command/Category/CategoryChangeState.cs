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
/// Change the state of the entity
/// </summary>
/// <param name="Id">Entity Id</param>
/// <param name="State">State to change</param>
public record CategoryChangeStateCommand(long Id, bool State) : IRequest<CategoryChangeStateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Category).ToLower()}")
    };
}


/// <inheritdoc />
public class CategoryChangeStateHandler : IRequestHandler<CategoryChangeStateCommand, CategoryChangeStateResponse>
{
    private readonly ILogger _logger;
    private readonly ICategoryService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public CategoryChangeStateHandler(ILogger<CategoryChangeStateHandler> logger, ICategoryService service)
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
    public async Task<CategoryChangeStateResponse> Handle(CategoryChangeStateCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.ChangeState(request.Id, request.State);
        _logger.LogInformation("Entity with id {RequestId} state change to {RequestState}", request.Id, request.State);
        return new CategoryChangeStateResponse { Data = result };
    }
}

/// <inheritdoc />
public record CategoryChangeStateResponse : CQRSResponse<bool>;