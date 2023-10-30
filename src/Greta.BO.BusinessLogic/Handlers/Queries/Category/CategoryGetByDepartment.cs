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
/// Get entity by department
/// </summary>
/// <param name="Id">Department id</param>
public record CategoryGetByDepartmentQuery(long Id) : IRequest<CategoryGetByDepartmentResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Category).ToLower()}")
    };

    /// <inheritdoc />
    public string CacheKey => $"CategoryGetByDepartment{Id}";
}

/// <inheritdoc />
public class CategoryGetByDepartmentHandler : IRequestHandler<CategoryGetByDepartmentQuery, CategoryGetByDepartmentResponse>
{
    private readonly IMapper _mapper;
    private readonly ICategoryService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public CategoryGetByDepartmentHandler(ICategoryService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<CategoryGetByDepartmentResponse> Handle(CategoryGetByDepartmentQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.GetByDepartment(request.Id);
        return new CategoryGetByDepartmentResponse { Data = _mapper.Map<List<CategoryModel>>(entity) };
    }
}

/// <inheritdoc />
public record CategoryGetByDepartmentResponse : CQRSResponse<List<CategoryModel>>;