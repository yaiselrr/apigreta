using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.ScaleCategory;

/// <summary>
/// Get all entities
/// </summary>
public record ScaleCategoryGetAllQuery : IRequest<ScaleCategoryGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_scale_category")
    };
}

/// <inheritdoc />
public class ScaleCategoryGetAllHandler : IRequestHandler<ScaleCategoryGetAllQuery, ScaleCategoryGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly IScaleCategoryService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ScaleCategoryGetAllHandler(IScaleCategoryService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ScaleCategoryGetAllResponse> Handle(ScaleCategoryGetAllQuery request,
        CancellationToken cancellationToken = default)
    {
        var entities = await _service.Get();
        return new ScaleCategoryGetAllResponse { Data = _mapper.Map<List<ScaleCategoryModel>>(entities) };
    }
}

/// <inheritdoc />
public record ScaleCategoryGetAllResponse : CQRSResponse<List<ScaleCategoryModel>>;