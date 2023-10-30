using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.RoundingTable;

/// <summary>
/// Command for update RoundingTable
/// </summary>
/// <param name="Id"></param>
/// <param name="Entity"></param>
public record RoundingTableUpdateCommand(long Id, RoundingTableModel Entity) : IRequest<RoundingTableUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements { get; } = new();
    
    // public List<IRequirement> Requirements => new()
    // {
    //     new PermissionRequirement.Requirement($"add_edit_{nameof(RoundingTable).ToLower()}")
    // };
}

/// <summary>
/// <inheritdoc cref="RoundingTableUpdateHandler"/>
/// </summary>
public class RoundingTableUpdateHandler : IRequestHandler<RoundingTableUpdateCommand, RoundingTableUpdateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IRoundingTableService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public RoundingTableUpdateHandler(
        ILogger<RoundingTableUpdateHandler> logger,
        IRoundingTableService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// <inheritdoc cref="RoundingTableUpdateResponse"/>
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<RoundingTableUpdateResponse> Handle(RoundingTableUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.RoundingTable>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("RoundingTable {RoundingTableId} update successfully", request.Id);
        return new RoundingTableUpdateResponse { Data = success };
    }
}

/// <summary>
/// <inheritdoc cref="RoundingTableUpdateResponse"/>
/// </summary>
public record RoundingTableUpdateResponse : CQRSResponse<bool>;