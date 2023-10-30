using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.RoundingTableQueries;

/// <summary>
/// Get all entities
/// </summary>
public record RoundingTableGetAllQuery : IRequest<RoundingTableGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements { get; } = new();
    // public List<IRequirement> Requirements => new()
    // {
    //     new PermissionRequirement.Requirement($"view_{nameof(RoundingTable).ToLower()}")
    // };
}

/// <inheritdoc />
public class RoundingTableGetAllHandler : IRequestHandler<RoundingTableGetAllQuery, RoundingTableGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly IRoundingTableService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public RoundingTableGetAllHandler(IRoundingTableService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<RoundingTableGetAllResponse> Handle(RoundingTableGetAllQuery request, CancellationToken cancellationToken)
    {
        var entities = await _service.Get();
        return new RoundingTableGetAllResponse { Data = _mapper.Map<List<RoundingTableModel>>(entities) };
    }
}

/// <inheritdoc />
public record RoundingTableGetAllResponse : CQRSResponse<List<RoundingTableModel>>;