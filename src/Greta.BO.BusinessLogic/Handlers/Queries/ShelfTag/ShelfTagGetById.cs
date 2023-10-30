using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Core.Caching;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.ShelfTag;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Id"></param>
public record ShelfTagGetByIdQuery(long Id) : IRequest<ShelfTagGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_shelf_tags")
    };

    /// <inheritdoc />
    public string CacheKey => $"ShelfTagGetById{Id}";
}

/// <inheritdoc />
public class ShelfTagGetByIdHandler : IRequestHandler<ShelfTagGetByIdQuery, ShelfTagGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IShelfTagService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ShelfTagGetByIdHandler(IShelfTagService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ShelfTagGetByIdResponse> Handle(ShelfTagGetByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(new GetByIdSpec<Api.Entities.ShelfTag>(request.Id), cancellationToken);
        var data = _mapper.Map<ShelfTagModel>(entity);
        return new ShelfTagGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record ShelfTagGetByIdResponse : CQRSResponse<ShelfTagModel>;