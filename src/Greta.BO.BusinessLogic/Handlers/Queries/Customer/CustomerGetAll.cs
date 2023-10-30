using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Customer;

/// <summary>
/// Get all entities
/// </summary>
public record CustomerGetAllQuery : IRequest<CustomerGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Customer).ToLower()}")
    };
}

/// <inheritdoc />
public class CustomerGetAllHandler : IRequestHandler<CustomerGetAllQuery, CustomerGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly ICustomerService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public CustomerGetAllHandler(ICustomerService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<CustomerGetAllResponse> Handle(CustomerGetAllQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.Get();
        return new CustomerGetAllResponse { Data = _mapper.Map<List<CustomerModel>>(entities) };
    }
}

/// <inheritdoc />
public record CustomerGetAllResponse : CQRSResponse<List<CustomerModel>>;