using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Category;

/// <summary>
/// Get all entities
/// </summary>
public record CategoryGetAllQuery(bool ToExport = false) : IRequest<CategoryGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Category).ToLower()}")
    };
}

/// <inheritdoc />
public class CategoryGetAllHandler : IRequestHandler<CategoryGetAllQuery, CategoryGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly ICategoryService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public CategoryGetAllHandler(ICategoryService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<CategoryGetAllResponse> Handle(CategoryGetAllQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.Get();
        return new CategoryGetAllResponse { Data = _mapper.Map<List<CategoryModel>>(entities) };
    }
}

/// <inheritdoc />
public record CategoryGetAllResponse : CQRSResponse<List<CategoryModel>>;