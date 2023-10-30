using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Core.Caching;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Category;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Id">Category id</param>
public record CategoryGetByIdQuery(long Id) : IRequest<CategoryGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Category).ToLower()}")
    };

    /// <summary>
    /// 
    /// </summary>
    public string CacheKey => $"CategoryGetById{Id}";
}

/// <inheritdoc />
public class CategoryGetByIdHandler : IRequestHandler<CategoryGetByIdQuery, CategoryGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly ICategoryService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public CategoryGetByIdHandler(ICategoryService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<CategoryGetByIdResponse> Handle(CategoryGetByIdQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(request.Id);
        var data = _mapper.Map<CategoryModel>(entity);
        return new CategoryGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record CategoryGetByIdResponse : CQRSResponse<CategoryModel>;