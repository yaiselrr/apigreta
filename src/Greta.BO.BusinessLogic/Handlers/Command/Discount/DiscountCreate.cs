using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using LanguageExt;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Discount;

/// <summary>
/// Command to create Discount Entity
/// </summary>
/// <param name="Entity"></param>
public record DiscountCreateCommand(DiscountModel Entity) : IRequest<DiscountCreateResponse>, IAuthorizable
{
    /// <inheritdoc/>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Discount).ToLower()}")
    };
}

/// <inheritdoc/>
public class Validator : AbstractValidator<DiscountCreateCommand>
{
    private readonly IDiscountService _service;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public Validator(IDiscountService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Discount name already exists");

        RuleFor(x => x.Entity.Type).Must(OnlyThisValues).WithMessage("Discount type is or Fix or Percent");
        RuleFor(x => x.Entity.Value).GreaterThan(0);
    }

    private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
    {
        var upcExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Discount>(name), cancellationToken);
        return upcExist == null;
    }

    private bool OnlyThisValues(DiscountType type)
    {
        return type == DiscountType.FIX || type == DiscountType.PERCENT;
    }
}

/// <inheritdoc/>
public class DiscountCreateHandler : IRequestHandler<DiscountCreateCommand, DiscountCreateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IDiscountService _service;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public DiscountCreateHandler(ILogger<DiscountCreateHandler> logger, IDiscountService service, IMapper mapper)
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
    public async Task<DiscountCreateResponse> Handle(DiscountCreateCommand request, CancellationToken cancellationToken=default)
    {
        var entity = _mapper.Map<Api.Entities.Discount>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation("Create Discount {Name} for user {UserCreatorId}", result.Name, result.UserCreatorId);
        return new DiscountCreateResponse { Data = !result.IsNull() };
    }
}

/// <inheritdoc/>
public record DiscountCreateResponse : CQRSResponse<bool>;
