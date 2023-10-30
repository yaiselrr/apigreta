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

namespace Greta.BO.BusinessLogic.Handlers.Queries.Department;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Id">Department id</param>
public record DepartmentGetByIdQuery(long Id) : IRequest<DepartmentGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Department).ToLower()}")
    };

    /// <inheritdoc />
    public string CacheKey => $"DepartmentGetById{Id}";
}

/// <inheritdoc />
public class DepartmentGetByIdHandler : IRequestHandler<DepartmentGetByIdQuery, DepartmentGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IDepartmentService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public DepartmentGetByIdHandler(IDepartmentService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<DepartmentGetByIdResponse> Handle(DepartmentGetByIdQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(request.Id);
        var data = _mapper.Map<DepartmentModel>(entity);
        return data == null ? null : new DepartmentGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record DepartmentGetByIdResponse : CQRSResponse<DepartmentModel>;