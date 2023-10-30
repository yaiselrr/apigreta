using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Family;

/// <summary>
/// Get all entities
/// </summary>
public record FamilyGetAllQuery : IRequest<FamilyGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Family).ToLower()}")
    };
}

/// <inheritdoc />
public class FamilyGetAllHandler : IRequestHandler<FamilyGetAllQuery, FamilyGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly IFamilyService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public FamilyGetAllHandler(IFamilyService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<FamilyGetAllResponse> Handle(FamilyGetAllQuery request, CancellationToken cancellationToken)
    {
        var entities = await _service.Get();
        return new FamilyGetAllResponse { Data = _mapper.Map<List<FamilyModel>>(entities) };
    }
}

/// <inheritdoc />
public record FamilyGetAllResponse : CQRSResponse<List<FamilyModel>>;