using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using LanguageExt;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.CutListTemplate;

/// <summary>
/// Command to create CutListTemplate Entity
/// </summary>
/// <param name="Entity"></param>
public record CutListTemplateCreateCommand(CutListTemplateModel Entity) : IRequest<CutListTemplateCreateResponse>, IAuthorizable
{
    /// <inheritdoc/>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc/>
public class Validator : AbstractValidator<CutListTemplateCreateCommand>
{
    private readonly ICutListTemplateService _service;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public Validator(ICutListTemplateService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("CutListTemplate name already exists");      
    }

    private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
    {
        var mixAndMatchExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.CutListTemplate>(name), cancellationToken);
        return mixAndMatchExist == null;
    }    
}

/// <inheritdoc/>
public class CutListTemplateCreateHandler : IRequestHandler<CutListTemplateCreateCommand, CutListTemplateCreateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly ICutListTemplateService _service;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public CutListTemplateCreateHandler(ILogger<CutListTemplateCreateHandler> logger, ICutListTemplateService service, IMapper mapper)
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
    public async Task<CutListTemplateCreateResponse> Handle(CutListTemplateCreateCommand request, CancellationToken cancellationToken=default)
    {        
        var entity = _mapper.Map<Api.Entities.CutListTemplate>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation("Create CutListTemplate {Name} for user {UserCreatorId}", result.Name, result.UserCreatorId);
        return new CutListTemplateCreateResponse { Data = !result.IsNull() };
    }
}

/// <inheritdoc/>
public record CutListTemplateCreateResponse : CQRSResponse<bool>;
