using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Department;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Perishable">Bool value</param>
public record DepartmentGetPerishablesQuery(bool Perishable) : IRequest<DepartmentGetPerishablesResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Department).ToLower()}")
    };
}

/// <inheritdoc />
public class DepartmentGetPerishablesHandler : IRequestHandler<DepartmentGetPerishablesQuery, DepartmentGetPerishablesResponse>
{
    private readonly IMapper _mapper;
    private readonly IDepartmentService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public DepartmentGetPerishablesHandler(IDepartmentService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<DepartmentGetPerishablesResponse> Handle(DepartmentGetPerishablesQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.Get(request.Perishable);
        return new DepartmentGetPerishablesResponse { Data = _mapper.Map<List<DepartmentModel>>(entities) };
    }
}

/// <inheritdoc />
public record DepartmentGetPerishablesResponse : CQRSResponse<List<DepartmentModel>>;