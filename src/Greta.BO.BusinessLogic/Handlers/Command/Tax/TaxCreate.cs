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

namespace Greta.BO.BusinessLogic.Handlers.Command.Tax;

/// <summary>
/// Create entity
/// </summary>
/// <param name="Entity">Tax entity</param>
public record TaxCreateCommand(TaxModel Entity) : IRequest<TaxCreateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Tax).ToLower()}")
    };
}

/// <inheritdoc />
public class TaxCreateValidator : AbstractValidator<TaxCreateCommand>
{
    private readonly ITaxService _service;

    /// <inheritdoc />
    public TaxCreateValidator(ITaxService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Tax name already exists.");

        RuleFor(x => x.Entity.Description)
                    .NotEmpty()
                    .Length(3, 254);

        RuleFor(x => x.Entity.Type)
            .Must(OnlyThisValues)
            .WithMessage("Tax type is or Fix or Percent.");

        RuleFor(x => x.Entity.Value)
            .NotNull();
    }

    private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
    {
        var upcExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Tax>(name), cancellationToken);
        return upcExist == null;
    }

    private bool OnlyThisValues(TaxType type)
    {
        if (((int)type) >= 0)
        {
            return type == TaxType.FIX || type == TaxType.PERCENT;
        }

        return false;
        // return TaxType.TryParse(type.ToString(), out TaxType _);
    }
}

/// <inheritdoc />
public class TaxCreateHandler : IRequestHandler<TaxCreateCommand, TaxCreateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly ITaxService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public TaxCreateHandler(
        ILogger<TaxCreateHandler> logger,
        ITaxService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<TaxCreateResponse> Handle(TaxCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Tax>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation("Create Tax {TaxName} for user {UserId}", result.Name, result.UserCreatorId);
        return new TaxCreateResponse { Data = _mapper.Map<TaxModel>(result) };
    }
}

/// <inheritdoc />
public record TaxCreateResponse : CQRSResponse<TaxModel>;