using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Core.Caching;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Customer;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Id">Tax id</param>
public record CustomerGetByIdQuery(long Id) : IRequest<CustomerGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Customer).ToLower()}")
    };

    /// <inheritdoc />
    public string CacheKey => $"CustomerGetById{Id}";
}

/// <inheritdoc />
public class CustomerGetByIdHandler : IRequestHandler<CustomerGetByIdQuery, CustomerGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly ICustomerService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public CustomerGetByIdHandler(ICustomerService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<CustomerGetByIdResponse> Handle(CustomerGetByIdQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(request.Id);
        var data = _mapper.Map<CustomerModel>(entity);
        return data == null ? null : new CustomerGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record CustomerGetByIdResponse : CQRSResponse<CustomerModel>;