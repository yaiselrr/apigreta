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

namespace Greta.BO.BusinessLogic.Handlers.Command.PriceBatchDetail;

/// <summary>
/// Command for Update PriceBatcDetail
/// </summary>
/// <param name="Id"></param>
/// <param name="Entity"></param>
public record PriceBatchDetailUpdateCommand(long Id, PriceBatchDetailModel Entity) : IRequest<PriceBatchDetailUpdateResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        // new PermissionRequirement.Requirement($"add_edit_price_batch_detail")
    };
}


/// <inheritdoc />
public class PriceBatchDetailUpdateHandler : IRequestHandler<PriceBatchDetailUpdateCommand, PriceBatchDetailUpdateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IPriceBatchDetailService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public PriceBatchDetailUpdateHandler(
        ILogger<PriceBatchDetailUpdateHandler> logger,
        IPriceBatchDetailService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<PriceBatchDetailUpdateResponse> Handle(PriceBatchDetailUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.PriceBatchDetail>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("PriceBatchDetail {RequestId} update successfully",request.Id);
        return new PriceBatchDetailUpdateResponse {Data = success};
    }
}


/// <inheritdoc/>
public record PriceBatchDetailUpdateResponse : CQRSResponse<bool>;

