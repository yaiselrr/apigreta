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

namespace Greta.BO.BusinessLogic.Handlers.Command.Department;

/// <summary>
/// Update Entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="Entity">New Entity</param>
public record DepartmentUpdateCommand(long Id, DepartmentModel Entity) : IRequest<DepartmentUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Department).ToLower()}")
    };
}

/// <inheritdoc />
public class DepartmentUpdateValidator : AbstractValidator<DepartmentUpdateCommand>
{
    private readonly IDepartmentService _service;

    /// <inheritdoc />
    public DepartmentUpdateValidator(IDepartmentService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Department name already exists.");
    }

    private async Task<bool> NameUnique(DepartmentUpdateCommand command, string name, CancellationToken cancellationToken)
    {
        var regionExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Department>(name, command.Id), cancellationToken);
        return regionExist == null;
    }
}

/// <inheritdoc />
public class DepartmentUpdateHandler : IRequestHandler<DepartmentUpdateCommand, DepartmentUpdateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IDepartmentService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public DepartmentUpdateHandler(
        ILogger<DepartmentUpdateHandler> logger,
        IDepartmentService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<DepartmentUpdateResponse> Handle(DepartmentUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Department>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("Department {DepartmentId} update successfully", request.Id);
        return new DepartmentUpdateResponse { Data = success };
    }
}

/// <inheritdoc />
public record DepartmentUpdateResponse : CQRSResponse<bool>;