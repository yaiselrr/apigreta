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
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Discount;

/// <summary>
/// Update entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="Entity">New entity</param>
public record DiscountUpdateCommand
    (long Id, DiscountModel Entity) : IRequest<DiscountUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Discount).ToLower()}")
    };
}

/// <inheritdoc />
public class DiscountUpdateValidator : AbstractValidator<DiscountUpdateCommand>
{
    private readonly IDiscountService _service;

    /// <inheritdoc />
    public DiscountUpdateValidator(IDiscountService service)
    {
        _service = service;

        RuleFor(x => x.Entity.Name)
                     .NotEmpty()
                     .Length(3, 64)
                     .MustAsync(NameUnique).WithMessage("Discount name already exists");

        RuleFor(x => x.Entity.Type).Must(OnlyThisValues).WithMessage("Discount type is or Fix or Percent");
        RuleFor(x => x.Entity.Value).NotNull();
    }

    private async Task<bool> NameUnique(DiscountUpdateCommand command, string name, CancellationToken cancellationToken)
    {
        var discountExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Discount>(name, command.Id), cancellationToken);
        return discountExist == null;
    }

    private bool OnlyThisValues(DiscountType type)
    {
        return type == DiscountType.FIX || type == DiscountType.PERCENT;
    }
}

/// <inheritdoc />
public class DiscountUpdateHandler : IRequestHandler<DiscountUpdateCommand, DiscountUpdateResponse>
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
    public DiscountUpdateHandler(
        ILogger<DiscountUpdateHandler> logger,
        IDiscountService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<DiscountUpdateResponse> Handle(DiscountUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Discount>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("Discount {DiscountId} update successfully", request.Id);
        return new DiscountUpdateResponse { Data = success };
    }
}

/// <inheritdoc />
public record DiscountUpdateResponse : CQRSResponse<bool>;