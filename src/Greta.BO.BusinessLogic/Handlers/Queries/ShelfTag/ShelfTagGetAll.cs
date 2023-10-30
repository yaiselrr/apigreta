using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.ShelfTag;

/// <summary>
/// Get all entities
/// </summary>
public record ShelfTagGetAllQuery : IRequest<ShelfTagGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_shelf_tags")
    };
}

/// <inheritdoc />
public class ShelfTagGetAllHandler : IRequestHandler<ShelfTagGetAllQuery, ShelfTagGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly IShelfTagService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ShelfTagGetAllHandler(IShelfTagService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ShelfTagGetAllResponse> Handle(ShelfTagGetAllQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.Get();
                return new ShelfTagGetAllResponse {Data = _mapper.Map<List<ShelfTagModel>>(entities)};
    }
}

/// <inheritdoc />
public record ShelfTagGetAllResponse : CQRSResponse<List<ShelfTagModel>>;