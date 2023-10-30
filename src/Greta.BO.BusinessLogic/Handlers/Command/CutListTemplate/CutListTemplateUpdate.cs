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
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.CutListTemplate;

/// <summary>
/// Command for update CutListTemplate Entity
/// </summary>
/// <param name="Id"></param>
/// <param name="Entity"></param>
public record CutListTemplateUpdateCommand(long Id, CutListTemplateModel Entity) : IRequest<CutListTemplateUpdateResponse>, IAuthorizable
{
    /// <inheritdoc/>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Animal).ToLower()}")
    };
}
/// <inheritdoc/>
public class CutListTemplateUpdateValidator : AbstractValidator<CutListTemplateUpdateCommand>
{
    private readonly ICutListTemplateService _service;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public CutListTemplateUpdateValidator(ICutListTemplateService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("CutListTemplate name already exists");       
    }

    private async Task<bool> NameUnique(CutListTemplateUpdateCommand command, string name, CancellationToken cancellationToken)
    {
        var mixAndMatchExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.CutListTemplate>(name, command.Id), cancellationToken);
        return mixAndMatchExist == null;
    }   
}
/// <inheritdoc/>
public class CutListTemplateUpdateHandler : IRequestHandler<CutListTemplateUpdateCommand, CutListTemplateUpdateResponse>
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
    public CutListTemplateUpdateHandler(
        ILogger<CutListTemplateUpdateHandler> logger,
        ICutListTemplateService service,
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
    public async Task<CutListTemplateUpdateResponse> Handle(CutListTemplateUpdateCommand request, CancellationToken cancellationToken)
    {       
        var entity = _mapper.Map<Api.Entities.CutListTemplate>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("CutListTemplate {RequestId} update successfully", request.Id);
        return new CutListTemplateUpdateResponse { Data = success};
    }
}
///<inheritdoc/>
public record CutListTemplateUpdateResponse : CQRSResponse<bool>;
