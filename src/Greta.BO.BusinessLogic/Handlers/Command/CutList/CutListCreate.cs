using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.CutList;

/// <summary>
/// Create entity
/// </summary>
/// <param name="Entity"></param>
public record CutListCreateCommand(CutListModel Entity) : IRequest<CutListCreateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc />
public class CutListCreateHandler : IRequestHandler<CutListCreateCommand, CutListCreateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly ICutListService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public CutListCreateHandler(
        ILogger<CutListCreateHandler> logger,
        ICutListService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<CutListCreateResponse> Handle(CutListCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.CutList>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation(
            "Create CutList for animal tag {AnimalTag} and customer {CustomerId} for the user {UserId}",
            result.AnimalId, result.CustomerId, result.UserCreatorId);
        return new CutListCreateResponse { Data = _mapper.Map<CutListModel>(result) };
    }
}

/// <inheritdoc />
public record CutListCreateResponse : CQRSResponse<CutListModel>;