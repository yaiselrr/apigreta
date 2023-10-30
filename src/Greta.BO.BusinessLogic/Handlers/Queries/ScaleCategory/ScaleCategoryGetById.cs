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

namespace Greta.BO.BusinessLogic.Handlers.Queries.ScaleCategory;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
public record ScaleCategoryGetByIdQuery(long Id) : IRequest<ScaleCategoryGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_scale_category")
    };

    /// <inheritdoc />
    public string CacheKey => $"ScaleCategoryGetById{Id}";
}

/// <inheritdoc />
public class ScaleCategoryGetByIdHandler : IRequestHandler<ScaleCategoryGetByIdQuery, ScaleCategoryGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IScaleCategoryService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ScaleCategoryGetByIdHandler(IScaleCategoryService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ScaleCategoryGetByIdResponse> Handle(ScaleCategoryGetByIdQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(new GetByIdSpec<Api.Entities.ScaleCategory>(request.Id), cancellationToken);
        var data = _mapper.Map<ScaleCategoryModel>(entity);
        return new ScaleCategoryGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record ScaleCategoryGetByIdResponse : CQRSResponse<ScaleCategoryModel>;