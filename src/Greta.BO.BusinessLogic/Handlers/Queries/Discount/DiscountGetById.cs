using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Discount;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Id"></param>
public record DiscountGetByIdQuery(long Id) : IRequest<DiscountGetByIdResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Discount).ToLower()}")
    };
}

/// <inheritdoc />
public class DiscountGetByIdHandler : IRequestHandler<DiscountGetByIdQuery, DiscountGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IDiscountService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public DiscountGetByIdHandler(IDiscountService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<DiscountGetByIdResponse> Handle(DiscountGetByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(request.Id);
        var data = _mapper.Map<DiscountModel>(entity);
        return new DiscountGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record DiscountGetByIdResponse : CQRSResponse<DiscountModel>;