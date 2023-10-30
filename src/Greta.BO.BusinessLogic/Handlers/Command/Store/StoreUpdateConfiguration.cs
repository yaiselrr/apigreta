namespace Greta.BO.BusinessLogic.Handlers.Command.Store;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Authorization;
using Authorization.Requirements;
using DeviceConfiguration;
using Models.Dto;
using Service;
using MediatR;
using Microsoft.Extensions.Logging;

/// <summary>
/// Update configuration entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="Entity">New entity</param>
public record StoreUpdateConfigurationCommand(long Id, StoreModel Entity) : IRequest<StoreUpdateConfigurationResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Store).ToLower()}")
    };
}

/// <inheritdoc />
public record StoreUpdateConfigurationResponse : CQRSResponse<bool>;

/// <inheritdoc />
public class StoreUpdateConfigurationHandler : IRequestHandler<StoreUpdateConfigurationCommand, StoreUpdateConfigurationResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IStoreService _service;
    private readonly IMediator _mediator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mediator"></param>
    /// <param name="mapper"></param>
    public StoreUpdateConfigurationHandler(
        ILogger<StoreUpdateConfigurationHandler> logger,
        IStoreService service,
        IMediator mediator,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<StoreUpdateConfigurationResponse> Handle(StoreUpdateConfigurationCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Store>(request.Entity);
        var success = await _service.PutConfiguration(request.Id, entity);
        
        //notify all clients 
        await _mediator.Send(new SendToDeviceCommand(request.Id, null), cancellationToken);
        
        _logger.LogInformation("Store configuration {StoreId} update successfully", request.Id);
        return new StoreUpdateConfigurationResponse {Data = success};
    }
}



