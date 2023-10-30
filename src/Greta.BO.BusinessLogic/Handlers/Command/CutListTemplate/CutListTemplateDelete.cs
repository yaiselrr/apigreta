using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.CutListTemplate;

/// <summary>
/// Command for delete CutListTemplate entity
/// </summary>
/// <param name="Id"></param>
public record CutListTemplateDeleteCommand(long Id) : IRequest<CutListTemplateDeleteResponse>, IAuthorizable
{
    /// <inheritdoc/>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc />
public class CutListTemplateValidator : AbstractValidator<CutListTemplateDeleteCommand>
{
    private readonly ICutListTemplateService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public CutListTemplateValidator(ICutListTemplateService service)
    {
        _service = service;

        RuleFor(x => x.Id)
            .MustAsync(CanDeleted)
            .WithMessage($"This CutListTemplate cannot be deleted because it is associated with another element");
    }

    private async Task<bool> CanDeleted(long id, CancellationToken cancellationToken)
    {
        return await _service.CanDeleted(id);
    }
}

/// <inheritdoc/>
public class CutListTemplateDeleteHandler : IRequestHandler<CutListTemplateDeleteCommand, CutListTemplateDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly ICutListTemplateService _service;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>   
    public CutListTemplateDeleteHandler(ILogger<CutListTemplateDeleteHandler> logger, ICutListTemplateService service)
    {
        _logger = logger;
        _service = service;        
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CutListTemplateDeleteResponse> Handle(CutListTemplateDeleteCommand request, CancellationToken cancellationToken=default)
    {
        if (request.Id < 1)
        {
            _logger.LogInformation("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new CutListTemplateDeleteResponse { Data = result};
    }
}

/// <inheritdoc/>
public record CutListTemplateDeleteResponse : CQRSResponse<bool>;
