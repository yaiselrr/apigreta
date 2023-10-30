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

namespace Greta.BO.BusinessLogic.Handlers.Command.Customer;

/// <summary>
/// Create entity
/// </summary>
/// <param name="Entity"></param>
public record CustomerCreateCommand(CustomerModel Entity) : IRequest<CustomerCreateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Customer).ToLower()}")
    };
}

/// <inheritdoc />
public class Validator : AbstractValidator<CustomerCreateCommand>
{
    /// <inheritdoc />
    public Validator()
    {
        RuleFor(x => x.Entity.FirstName)
                    .NotEmpty()
                    .Length(3, 64);

        RuleFor(x => x.Entity.LastName)
            .NotEmpty()
            .Length(3, 64);

        RuleFor(x => x.Entity.Phone)
            .NotEmpty()
            .Length(3, 20);

        RuleFor(x => x.Entity.Email)
            .NotEmpty()
            .Length(3, 100);
    }
}

/// <inheritdoc />
public class CustomerCreateHandler : IRequestHandler<CustomerCreateCommand, CustomerCreateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly ICustomerService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public CustomerCreateHandler(
        ILogger<CustomerCreateHandler> logger,
        ICustomerService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<CustomerCreateResponse> Handle(CustomerCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Customer>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation("Create Customer {CustomerName} for user {UserId}", result.FirstName, result.UserCreatorId);
        return new CustomerCreateResponse { Data = _mapper.Map<CustomerModel>(result) };
    }
}

/// <inheritdoc />
public record CustomerCreateResponse : CQRSResponse<CustomerModel>;