using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Core.Caching;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.MixAndMatchDto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.MixAndMatch;

/// <summary>
/// Query for get by id MixAndMatch entity
/// </summary>
/// <param name="Id"></param>
public record MixAndMatchGetByIdQuery(long Id) : IRequest<MixAndMatchGetByIdResponse>, IAuthorizable, ICacheable
{
/// <inheritdoc/>
    public List<IRequirement> Requirements => new() {
        new PermissionRequirement.Requirement($"view_mix_and_match")
    };

    /// <inheritdoc/>
    public string CacheKey => $"MixAndMatchGetById{Id}";
}

///<inheritdoc/>
public class MixAndMatchGetByIdHandler : IRequestHandler<MixAndMatchGetByIdQuery, MixAndMatchGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IMixAndMatchService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public MixAndMatchGetByIdHandler(IMixAndMatchService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<MixAndMatchGetByIdResponse> Handle(MixAndMatchGetByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _service.Get(request.Id);
        var data = _mapper.Map<MixAndMatchGetByIdModel>(entity);
        return data == null ? null : new MixAndMatchGetByIdResponse { Data = data};
    }
}

///<inheritdoc/>
public record MixAndMatchGetByIdResponse : CQRSResponse<MixAndMatchGetByIdModel>;
