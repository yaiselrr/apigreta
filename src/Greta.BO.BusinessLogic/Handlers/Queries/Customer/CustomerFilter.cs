using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.CustomerSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Customer;

/// <summary>
/// Query for filter the Customer entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record CustomerFilterQuery
    (int CurrentPage, int PageSize, CustomerSearchModel Filter) : IRequest<CustomerFilterResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Customer).ToLower()}")
    };
}

/// <inheritdoc />
public class CustomerFilterValidator : AbstractValidator<CustomerFilterQuery>
{
    /// <inheritdoc />
    public CustomerFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class CustomerFilterHandler : IRequestHandler<CustomerFilterQuery, CustomerFilterResponse>
{
    private readonly IMapper _mapper;
    private readonly ICustomerService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public CustomerFilterHandler(ICustomerService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<CustomerFilterResponse> Handle(CustomerFilterQuery request,
        CancellationToken cancellationToken = default)
    {
        var spec = new CustomerFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);
        return new CustomerFilterResponse { Data = _mapper.Map<Pager<CustomerModel>>(entities) };
    }
}

/// <inheritdoc />
public record CustomerFilterResponse : CQRSResponse<Pager<CustomerModel>>;