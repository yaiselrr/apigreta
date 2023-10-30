using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Department;

/// <summary>
/// Get all entities
/// </summary>
public record DepartmentGetAllQuery(bool ToExport = false) : IRequest<DepartmentGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Department).ToLower()}")
    };
}

/// <inheritdoc />
public class DepartmentGetAllHandler : IRequestHandler<DepartmentGetAllQuery, DepartmentGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly IDepartmentService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public DepartmentGetAllHandler(IDepartmentService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<DepartmentGetAllResponse> Handle(DepartmentGetAllQuery request,
        CancellationToken cancellationToken = default)
    {
        var entities = await _service.Get();
        return new DepartmentGetAllResponse { Data = _mapper.Map<List<DepartmentGetAllDto>>(entities) };
    }
}

/// <inheritdoc />
public record DepartmentGetAllResponse : CQRSResponse<List<DepartmentGetAllDto>>;

/// <inheritdoc />
public class DepartmentGetAllDto : IMapFrom<Api.Entities.Department>
{
    /// <summary>
    /// 
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int DepartmentId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool Perishable { get; set; }
}