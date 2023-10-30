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

namespace Greta.BO.BusinessLogic.Handlers.Queries.RoundingTableQueries;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Id">Tax id</param>
public record RoundingTableGetByIdQuery(long Id) : IRequest<RoundingTableGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements { get; } = new();
    // public List<IRequirement> Requirements => new()
    // {
    //     new PermissionRequirement.Requirement($"view_{nameof(RoundingTable).ToLower()}")
    // };

    /// <inheritdoc />
    public string CacheKey => $"RoundingTableGetById{Id}";
}

/// <inheritdoc />
public class RoundingTableGetByIdHandler : IRequestHandler<RoundingTableGetByIdQuery, RoundingTableGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IRoundingTableService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public RoundingTableGetByIdHandler(IRoundingTableService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<RoundingTableGetByIdResponse> Handle(RoundingTableGetByIdQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(new GetByIdSpec<Api.Entities.RoundingTable>(request.Id));
        var data = _mapper.Map<RoundingTableModel>(entity);
        return data == null ? null : new RoundingTableGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record RoundingTableGetByIdResponse : CQRSResponse<RoundingTableModel>;