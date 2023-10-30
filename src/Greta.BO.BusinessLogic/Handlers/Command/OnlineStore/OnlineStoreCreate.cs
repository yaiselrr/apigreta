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
/// Create entity
/// </summary>
/// <param name="Entity"></param>
public record OnlineStoreCreateCommand(OnlineStoreModel Entity) : IRequest<OnlineStoreCreateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Store).ToLower()}")
    };
}

/// <inheritdoc />
public class OnlineStoreCreateValidator : AbstractValidator<OnlineStoreCreateCommand>
{
    private readonly IOnlineStoreService _service;

    /// <inheritdoc />
    public OnlineStoreCreateValidator(IOnlineStoreService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("OnlineStore name already exists.");
    }

    private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
    {
        var OnlineStoreExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.OnlineStore>(name), cancellationToken);
        return OnlineStoreExist == null;
    }
}

/// <inheritdoc />
public class OnlineStoreCreateHandler : IRequestHandler<OnlineStoreCreateCommand, OnlineStoreCreateResponse>
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
    public OnlineStoreCreateHandler(
        ILogger<OnlineStoreCreateHandler> logger,
        IOnlineStoreService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<OnlineStoreCreateResponse> Handle(OnlineStoreCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.OnlineStore>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation("Create OnlineStore {OnlineStoreName} for user {UserId}", result.Name, result.UserCreatorId);
        return new OnlineStoreCreateResponse { Data = _mapper.Map<OnlineStoreModel>(result) };
    }
}

/// <inheritdoc />
public record OnlineStoreCreateResponse : CQRSResponse<OnlineStoreModel>;