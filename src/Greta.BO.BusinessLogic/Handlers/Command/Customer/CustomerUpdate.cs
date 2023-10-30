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
/// Update entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="Entity">New entity</param>
public record CustomerUpdateCommand
    (long Id, CustomerModel Entity) : IRequest<CustomerUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Customer).ToLower()}")
    };
}

/// <inheritdoc />
public class CustomerUpdateValidator : AbstractValidator<CustomerUpdateCommand>
{
    /// <inheritdoc />
    public CustomerUpdateValidator()
    {
        RuleFor(x => x.Entity.FirstName)
                    .NotEmpty()
                    .Length(3, 64);

        RuleFor(x => x.Entity.LastName)
            .NotEmpty()
            .Length(3, 64);

        RuleFor(x => x.Entity.Phone)
            .NotEmpty()
            .Length(3, 250);

        RuleFor(x => x.Entity.Email)
            .NotEmpty()
            .Length(3, 100);
    }
}

/// <inheritdoc />
public class CustomerUpdateHandler : IRequestHandler<CustomerUpdateCommand, CustomerUpdateResponse>
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
    public CustomerUpdateHandler(
        ILogger<CustomerUpdateHandler> logger,
        ICustomerService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<CustomerUpdateResponse> Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
    {
        request.Entity.Id = request.Id;
        var entity = _mapper.Map<Api.Entities.Customer>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("Customer {CustomerId} update successfully", request.Id);
        return new CustomerUpdateResponse { Data = success };
    }
}

/// <inheritdoc />
public record CustomerUpdateResponse : CQRSResponse<bool>;