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

namespace Greta.BO.BusinessLogic.Handlers.Command.Family;

/// <summary>
/// Command for update family
/// </summary>
/// <param name="Id"></param>
/// <param name="Entity"></param>
public record FamilyUpdateCommand(long Id, FamilyModel Entity) : IRequest<FamilyUpdateResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Family).ToLower()}")
    };
}

/// <summary>
/// <inheritdoc cref="FamilyUpdateValidator"/>
/// </summary>
public class FamilyUpdateValidator : AbstractValidator<FamilyUpdateCommand>
{
    /// <summary>
    /// 
    /// </summary>
    public FamilyUpdateValidator()
    {
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64);
    }
}

/// <summary>
/// <inheritdoc cref="FamilyUpdateHandler"/>
/// </summary>
public class FamilyUpdateHandler : IRequestHandler<FamilyUpdateCommand, FamilyUpdateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IFamilyService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public FamilyUpdateHandler(
        ILogger<FamilyUpdateHandler> logger,
        IFamilyService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// <inheritdoc cref="FamilyUpdateResponse"/>
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<FamilyUpdateResponse> Handle(FamilyUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Family>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("Family {FamilyId} update successfully", request.Id);
        return new FamilyUpdateResponse { Data = success };
    }
}

/// <summary>
/// <inheritdoc cref="FamilyUpdateResponse"/>
/// </summary>
public record FamilyUpdateResponse : CQRSResponse<bool>;