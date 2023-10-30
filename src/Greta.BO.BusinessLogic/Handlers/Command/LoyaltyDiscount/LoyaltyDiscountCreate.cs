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
/// Query for create new LoyaltyDiscount
/// </summary>
/// <param name="Entity"></param>
public record LoyaltyDiscountCreateCommand(LoyaltyDiscountCreateModel Entity) : IRequest<LoyaltyDiscountCreateResponse>, IAuthorizable
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
public class LoyaltyDiscountCreateValidator : AbstractValidator<LoyaltyDiscountCreateCommand>
{
    private readonly ILoyaltyDiscountService _service;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public LoyaltyDiscountCreateValidator(ILoyaltyDiscountService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64);
        RuleFor(x => x.Entity.StoreId)
            .MustAsync(OnlyOneStore).WithMessage("LoyaltyDiscount store already exists");
    }

    private async Task<bool> OnlyOneStore(LoyaltyDiscountCreateCommand command, long storeId, CancellationToken cancellationToken)
    {
        var loyaltyDiscountExist = await _service.Get(new LoyaltyDiscountGetByStoreIdSpec(storeId, command.Entity.Id));
        return loyaltyDiscountExist == null || loyaltyDiscountExist.Count == 0;
    }
}

///<inheritdoc/>
public class LoyaltyDiscountCreateHandler : IRequestHandler<LoyaltyDiscountCreateCommand, LoyaltyDiscountCreateResponse>
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
    public LoyaltyDiscountCreateHandler(ILogger<LoyaltyDiscountCreateHandler> logger, ILoyaltyDiscountService service, IMapper mapper)
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
    public async Task<LoyaltyDiscountCreateResponse> Handle(LoyaltyDiscountCreateCommand request, CancellationToken cancellationToken=default)
    {
        var entity = _mapper.Map<Api.Entities.LoyaltyDiscount>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation("Create LoyaltyDiscount {ResultName} for user {ResultUserCreatorId}", result.Name, result.UserCreatorId);
        return new LoyaltyDiscountCreateResponse { Data = _mapper.Map<LoyaltyDiscountCreateModel>(result)};
    }
}
///<inheritdoc/>
public record LoyaltyDiscountCreateResponse : CQRSResponse<LoyaltyDiscountCreateModel>;
