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

namespace Greta.BO.BusinessLogic.Handlers.Queries.Role;

/// <summary>
/// Query for get all rols entities
/// </summary>
public record RoleGetAllQuery : IRequest<RoleGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement(string.Format("view_{0}",nameof(Role).ToLower()))
    };
}

/// <inheritdoc />
public class RoleGetAllHandler : IRequestHandler<RoleGetAllQuery, RoleGetAllResponse>
{
    private readonly IMapper _mapper;

    private readonly IRoleService _service;

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public RoleGetAllHandler(IRoleService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }
    /// <inheritdoc />
    public async Task<RoleGetAllResponse> Handle(RoleGetAllQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.Get();
        return new RoleGetAllResponse { Data = _mapper.Map<List<RoleGetAllDto>>(entities)};
    }
}

/// <inheritdoc />
public record RoleGetAllResponse : CQRSResponse<List<RoleGetAllDto>>;

/// <inheritdoc />
public class RoleGetAllDto : IMapFrom<Api.Entities.Role>
{
    /// <summary>
    /// Role Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Role Name
    /// </summary>
    public string Name { get; set; }
}

