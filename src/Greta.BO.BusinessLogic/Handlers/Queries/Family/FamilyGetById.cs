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

namespace Greta.BO.BusinessLogic.Handlers.Queries.Family;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Id">Tax id</param>
public record FamilyGetByIdQuery(long Id) : IRequest<FamilyGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Family).ToLower()}")
    };

    /// <inheritdoc />
    public string CacheKey => $"FamilyGetById{Id}";
}

/// <inheritdoc />
public class FamilyGetByIdHandler : IRequestHandler<FamilyGetByIdQuery, FamilyGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IFamilyService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public FamilyGetByIdHandler(IFamilyService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<FamilyGetByIdResponse> Handle(FamilyGetByIdQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(new GetByIdSpec<Api.Entities.Family>(request.Id));
        var data = _mapper.Map<FamilyModel>(entity);
        return data == null ? null : new FamilyGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record FamilyGetByIdResponse : CQRSResponse<FamilyModel>;