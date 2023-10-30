using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using LanguageExt;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.PriceBatchDetail;

/// <summary>
/// Create entity
/// </summary>
/// <param name="Entity"></param>
public record PriceBatchDetailCreateCommand(PriceBatchDetailModel Entity) : IRequest<PriceBatchDetailCreateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        // new PermissionRequirement.Requirement($"add_edit_price_batch")
    };
}

/// <inheritdoc />
public class PriceBatchDetailCreateHandler : IRequestHandler<PriceBatchDetailCreateCommand, PriceBatchDetailCreateResponse>
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
    public PriceBatchDetailCreateHandler(
        ILogger<PriceBatchDetailCreateHandler> logger,
        IPriceBatchDetailService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<PriceBatchDetailCreateResponse> Handle(PriceBatchDetailCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.PriceBatchDetail>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation("Create PriceBatchDetail for user {UserId}", result.UserCreatorId);
        return new PriceBatchDetailCreateResponse { Data = !result.IsNull() };
        // return new PriceBatchDetailCreateResponse { Data = _mapper.Map<PriceBatchDetailModel>(result) };
    }
}

/// <inheritdoc />
public record PriceBatchDetailCreateResponse : CQRSResponse<bool>;