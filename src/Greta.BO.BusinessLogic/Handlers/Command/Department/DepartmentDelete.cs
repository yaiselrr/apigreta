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

namespace Greta.BO.BusinessLogic.Handlers.Command.Department;

/// <summary>
/// Delete entity by entity id
/// </summary>
/// <param name="Id"></param>
public record DepartmentDeleteCommand(long Id) : IRequest<DepartmentDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Department).ToLower()}")
    };
}

/// <inheritdoc />
public class DepartmentDeleteHandler : IRequestHandler<DepartmentDeleteCommand, DepartmentDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly IDepartmentService _service;

    /// <inheritdoc />
    public class Validator : AbstractValidator<DepartmentDeleteCommand>
    {
        private readonly IDepartmentService _service;

        /// <inheritdoc />
        public Validator(IDepartmentService service)
        {
            _service = service;

            RuleFor(x => x.Id)
                .MustAsync(CanDeleted)
                .WithMessage($"This department cannot be deleted because it is associated with another element.");
        }

        private async Task<bool> CanDeleted(long id, CancellationToken cancellationToken)
        {
            return await _service.CanDeleted(id);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public DepartmentDeleteHandler(
        ILogger<DepartmentDeleteHandler> logger,
        IDepartmentService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<DepartmentDeleteResponse> Handle(DepartmentDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!await _service.CanDeleted(request.Id))
        {
            throw new BusinessLogicException(
                "This Department cannot be deleted because it is associated with another element.");
        }

        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new DepartmentDeleteResponse { Data = result };
    }
}

/// <inheritdoc />
public record DepartmentDeleteResponse : CQRSResponse<bool>;