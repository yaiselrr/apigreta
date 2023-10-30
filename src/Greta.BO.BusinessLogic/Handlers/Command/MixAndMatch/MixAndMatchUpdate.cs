using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.MixAndMatchDto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.MixAndMatch;

/// <summary>
/// Command for update MixAndMatch Entity
/// </summary>
/// <param name="Id"></param>
/// <param name="Entity"></param>
public record MixAndMatchUpdateCommand(long Id, MixAndMatchModel Entity) : IRequest<MixAndMatchUpdateResponse>, IAuthorizable
{
    /// <inheritdoc/>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement("add_edit_mix_and_match")
    };
}
/// <inheritdoc/>
public class MixAndMatchUpdateValidator : AbstractValidator<MixAndMatchUpdateCommand>
{
    private readonly IMixAndMatchService _service;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public MixAndMatchUpdateValidator(IMixAndMatchService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("MixAndMatch name already exists");
        RuleFor(x => x.Entity.MixAndMatchType)
            .Must(ValidateType).WithMessage("Missing required fields");
    }

    private async Task<bool> NameUnique(MixAndMatchUpdateCommand command, string name, CancellationToken cancellationToken)
    {
        var mixAndMatchExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.MixAndMatch>(name, command.Id), cancellationToken);
        return mixAndMatchExist == null;
    }

    private bool ValidateType(MixAndMatchUpdateCommand command, MixAndMatchType type)
    {
        //validate all types with the corrects rules
        switch (type)
        {
            case MixAndMatchType.BuyAndGetFree:
                //We need at lease 2 elements to apply this match 
                if (command.Entity.QTY < 2) return false;
                //we need or one product or one family
                if (command.Entity.ProductIds.Count == 0 && command.Entity.FamilyIds.Count == 0) return false;
                return true;
            case MixAndMatchType.DiscountPercentage:
                //We need at lease 2 elements to apply this match 
                if (command.Entity.QTY < 1) return false; // ??? it can be only 1 element
                if (command.Entity.ProductIds.Count == 0 && command.Entity.FamilyIds.Count == 0) return false;
                if (command.Entity.Amount <= 0 || command.Entity.Amount > 100) return false;
                return true;
            case MixAndMatchType.FixedPriceDiscount:
                //We need at lease 2 elements to apply this match 
                if (command.Entity.QTY < 2) return false;
                //we need or one product or one family
                if (command.Entity.ProductIds.Count == 0 && command.Entity.FamilyIds.Count == 0) return false;
                if (command.Entity.Amount <= 0.01M) return false;
                return true;
            case MixAndMatchType.BuyOneGetFree:
                //We need at lease 2 elements to apply this match 
                if (command.Entity.QTY < 2) return false;
                // we need one trigger product for this type
                if (command.Entity.ProductBuyId < 0) return false;
                if (command.Entity.ProductIds.Count == 0 && command.Entity.FamilyIds.Count == 0) return false;
                return true;
            case MixAndMatchType.BuyItemAndCredit:
                //We need Amount to this type
                if (command.Entity.Amount < 0.01M) return false;
                if (command.Entity.ProductIds.Count == 0 && command.Entity.FamilyIds.Count == 0) return false;
                return true;
            case MixAndMatchType.BuyOneGetOneCheapFree:
                // we need one trigger product for this type
                if (command.Entity.ProductBuyId < 0) return false;
                return true;
            default:
                return false;
        }
    }
}
/// <inheritdoc/>
public class MixAndMatchUpdateHandler : IRequestHandler<MixAndMatchUpdateCommand, MixAndMatchUpdateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IMixAndMatchService _service;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public MixAndMatchUpdateHandler(
        ILogger<MixAndMatchUpdateHandler> logger,
        IMixAndMatchService service,
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
    public async Task<MixAndMatchUpdateResponse> Handle(MixAndMatchUpdateCommand request, CancellationToken cancellationToken)
    {
        if (request.Entity.ProductBuyId == -1) request.Entity.ProductBuyId = null;
        var entity = _mapper.Map<Api.Entities.MixAndMatch>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("MixAndMatch {RequestId} update successfully", request.Id);
        return new MixAndMatchUpdateResponse { Data = success};
    }
}
///<inheritdoc/>
public record MixAndMatchUpdateResponse : CQRSResponse<bool>;
