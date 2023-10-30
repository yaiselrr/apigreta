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

namespace Greta.BO.BusinessLogic.Handlers.Command.OnlineStore;

/// <summary>
/// Update entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="Entity">New entity</param>
public record OnlineStoreUpdateCommand
    (long Id, OnlineStoreModel Entity) : IRequest<OnlineStoreUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Store).ToLower()}")
    };
}

/// <inheritdoc />
public class OnlineStoreUpdateValidator : AbstractValidator<OnlineStoreUpdateCommand>
{
    private readonly IOnlineStoreService _service;

    /// <inheritdoc />
    public OnlineStoreUpdateValidator(IOnlineStoreService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("OnlineStore name already exists.");
        
        RuleFor(x => x.Entity.StoreId)
                    .NotEmpty();
    }

    private async Task<bool> NameUnique(OnlineStoreUpdateCommand command, string name, CancellationToken cancellationToken)
    {
        var OnlineStoreExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.OnlineStore>(name, command.Id), cancellationToken);
        return OnlineStoreExist == null;
    }
}

/// <inheritdoc />
public class OnlineStoreUpdateHandler : IRequestHandler<OnlineStoreUpdateCommand, OnlineStoreUpdateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IOnlineStoreService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public OnlineStoreUpdateHandler(
        ILogger<OnlineStoreUpdateHandler> logger,
        IOnlineStoreService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<OnlineStoreUpdateResponse> Handle(OnlineStoreUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.OnlineStore>(request.Entity);
        var success = await _service.Put(request.Id, entity);
        _logger.LogInformation("OnlineStore {OnlineStoreId} update successfully", request.Id);
        return new OnlineStoreUpdateResponse { Data = success };
    }
}

/// <inheritdoc />
public record OnlineStoreUpdateResponse : CQRSResponse<bool>;