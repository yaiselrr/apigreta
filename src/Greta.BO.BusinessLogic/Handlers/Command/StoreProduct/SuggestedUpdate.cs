using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.StoreProduct;

/// <summary>
/// Command for update order amount
/// </summary>
/// <param name="Id"></param>
/// <param name="ValueAmount"></param>
public record SuggestedUpdateCommand(long Id, decimal ValueAmount) : IRequest<SuggestedUpdateResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement("associate_product_store")
    };
}

///<inheritdoc/>
public class SuggestedUpdateValidator : AbstractValidator<SuggestedUpdateCommand>
{
}

///<inheritdoc/>
public class SuggestedUpdateHandler : IRequestHandler<SuggestedUpdateCommand, SuggestedUpdateResponse>
{
    private readonly ILogger _logger;
    private readonly IStoreProductService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public SuggestedUpdateHandler(ILogger<SuggestedUpdateHandler> logger, IStoreProductService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<SuggestedUpdateResponse> Handle(SuggestedUpdateCommand request, CancellationToken cancellationToken)
    {
        var success = await _service.PutByOrderAmount(request.Id, request.ValueAmount);
        _logger.LogInformation("Suggested {RequestId} update successfully", request.Id);
        return new SuggestedUpdateResponse { Data = success};
    }
}

///<inheritdoc/>
public record SuggestedUpdateResponse : CQRSResponse<bool>;
