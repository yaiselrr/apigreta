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
/// Update entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="Entity">Entity with new values</param>
public record TaxUpdateCommand
    (long Id, TaxModel Entity) : IRequest<TaxUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Tax).ToLower()}")
    };
}

/// <inheritdoc />
public class TaxUpdateValidator : AbstractValidator<TaxUpdateCommand>
{
    private readonly ITaxService _service;

    /// <inheritdoc />
    public TaxUpdateValidator(ITaxService service)
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

    private bool OnlyThisValues(TaxType type)
    {
        return type == TaxType.FIX || type == TaxType.PERCENT;
    }

    private async Task<bool> NameUnique(TaxUpdateCommand command, string name, CancellationToken cancellationToken)
    {
        var upcExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Tax>(name, command.Id), cancellationToken);
        return upcExist == null;
    }
}

/// <inheritdoc />
public class TaxUpdateHandler : IRequestHandler<TaxUpdateCommand, TaxUpdateResponse>
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
    public TaxUpdateHandler(
        ILogger<TaxUpdateHandler> logger,
        ITaxService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<TaxUpdateResponse> Handle(TaxUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Tax>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("Tax {TaxId} update successfully", request.Id);
        return new TaxUpdateResponse { Data = success };
    }
}

/// <inheritdoc />
public record TaxUpdateResponse : CQRSResponse<bool>;