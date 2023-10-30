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
/// Create entity
/// </summary>
/// <param name="Entity"></param>
public record DepartmentCreateCommand(DepartmentModel Entity) : IRequest<DepartmentCreateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Department).ToLower()}")
    };
}

/// <inheritdoc />
public class Validator : AbstractValidator<DepartmentCreateCommand>
{
    private readonly IDepartmentService _service;

    /// <inheritdoc />
    public Validator(IDepartmentService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Department name already exists.");
    }

    private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
    {
        var nameExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Department>(name), cancellationToken);
        return nameExist == null;
    }
}

/// <inheritdoc />
public class DepartmentCreateHandler : IRequestHandler<DepartmentCreateCommand, DepartmentCreateResponse>
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
    public DepartmentCreateHandler(
        ILogger<DepartmentCreateHandler> logger,
        IDepartmentService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<DepartmentCreateResponse> Handle(DepartmentCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Department>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation("Create Department {DepartmentName} for user {UserId}", result.Name, result.UserCreatorId);
        return new DepartmentCreateResponse { Data = _mapper.Map<DepartmentModel>(result) };
    }
}

/// <inheritdoc />
public record DepartmentCreateResponse : CQRSResponse<DepartmentModel>;