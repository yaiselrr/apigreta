using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.LoyaltyDiscountDto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.LoyaltyDiscountSpecs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.LoyaltyDiscount;

/// <summary>
/// Query for update a LoyaltyDiscount
/// </summary>
/// <param name="Id"></param>
/// <param name="Entity"></param>
public record LoyaltyDiscountUpdateCommand(long Id, LoyaltyDiscountUpdateModel Entity) : IRequest<LoyaltyDiscountUpdateResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement("add_edit_loyalty_discount")
    };
}

///<inheritdoc/>
public class LoyaltyDiscountUpdateValidator : AbstractValidator<LoyaltyDiscountUpdateCommand>
{
    private readonly ILoyaltyDiscountService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public LoyaltyDiscountUpdateValidator(ILoyaltyDiscountService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64);
        RuleFor(x => x.Entity.StoreId)
            .MustAsync(OnlyOneStore).WithMessage("LoyaltyDiscount store already exists");
    }

    private async Task<bool> OnlyOneStore(LoyaltyDiscountUpdateCommand command, long storeId, CancellationToken cancellationToken)
    {
        var loyaltyDiscountExist = await _service.Get(new LoyaltyDiscountGetByStoreIdSpec(storeId, command.Id));
        return loyaltyDiscountExist == null || loyaltyDiscountExist.Count == 0;
    }
}

///<inheritdoc/>
public class LoyaltyDiscountUpdateHandler : IRequestHandler<LoyaltyDiscountUpdateCommand, LoyaltyDiscountUpdateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly ILoyaltyDiscountService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public LoyaltyDiscountUpdateHandler(ILogger<LoyaltyDiscountUpdateHandler> logger, ILoyaltyDiscountService service, IMapper mapper)
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
    public async Task<LoyaltyDiscountUpdateResponse> Handle(LoyaltyDiscountUpdateCommand request, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Api.Entities.LoyaltyDiscount>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("LoyaltyDiscount {RequestId} update successfully", request.Id);
        return new LoyaltyDiscountUpdateResponse { Data = success};
    }
}

///<inheritdoc/>
public record LoyaltyDiscountUpdateResponse : CQRSResponse<bool>;
