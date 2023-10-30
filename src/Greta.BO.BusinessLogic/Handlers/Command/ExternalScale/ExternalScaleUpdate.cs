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
/// Update entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="Entity">New entity</param>
public record ExternalScaleUpdateCommand
    (long Id, ExternalScaleModel Entity) : IRequest<ExternalScaleUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_external_scale")
    };
}

/// <inheritdoc />
public class ExternalScaleUpdateValidator : AbstractValidator<ExternalScaleUpdateCommand>
{
    /// <inheritdoc />
    public ExternalScaleUpdateValidator()
    {
        RuleFor(x => x.Entity.Ip)
            .NotEmpty()
            .Length(3, 64);
    }
}

/// <inheritdoc />
public class ExternalScaleUpdateHandler : IRequestHandler<ExternalScaleUpdateCommand, ExternalScaleUpdateResponse>
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
    public ExternalScaleUpdateHandler(
        ILogger<ExternalScaleUpdateHandler> logger,
        IExternalScaleService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }
    
    /// <inheritdoc />
    public async Task<ExternalScaleUpdateResponse> Handle(ExternalScaleUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.ExternalScale>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("External Scale {External Scale Id} update successfully", request.Id);
        return new ExternalScaleUpdateResponse { Data = success };
    }
}

/// <inheritdoc />
public record ExternalScaleUpdateResponse : CQRSResponse<bool>;