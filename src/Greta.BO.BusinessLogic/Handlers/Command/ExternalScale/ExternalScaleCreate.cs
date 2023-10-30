using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.ExternalScale;

/// <summary>
/// Create entity
/// </summary>
/// <param name="Entity"></param>
public record ExternalScaleCreateCommand(ExternalScaleModel Entity) : IRequest<ExternalScaleCreateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_external_scale")
    };
}

/// <inheritdoc />
public class Validator : AbstractValidator<ExternalScaleCreateCommand>
{
    /// <inheritdoc />
    public Validator()
    {
        RuleFor(x => x.Entity.Ip)
            .NotEmpty()
            .Length(3, 64);

        RuleFor(x => x.Entity.StoreId).GreaterThan(0).WithMessage("Store must be selected");

        RuleFor(x => (int)x.Entity.ExternalScaleType).GreaterThanOrEqualTo(0).WithMessage("Scale Brand must be selected");
    }
}

/// <inheritdoc />
public class ExternalScaleCreateHandler : IRequestHandler<ExternalScaleCreateCommand, ExternalScaleCreateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IExternalScaleService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ExternalScaleCreateHandler(
        ILogger<ExternalScaleCreateHandler> logger,
        IExternalScaleService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ExternalScaleCreateResponse> Handle(ExternalScaleCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.ExternalScale>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation("Create External Scale {External Scale IP} for user {UserId}", result.Ip, result.UserCreatorId);
        return new ExternalScaleCreateResponse { Data = _mapper.Map<ExternalScaleModel>(result) };
    }
}

/// <inheritdoc />
public record ExternalScaleCreateResponse : CQRSResponse<ExternalScaleModel>;