using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.EFCore.Middleware;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.GiftCardCommands;

/// <summary>
/// 
/// </summary>
/// <param name="Entity"></param>
public record GiftCardCreateCommand(GiftCardModel Entity) : IRequest<GiftCardCreateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Store).ToLower()}")
    };
}

/// <summary>
///   Response after creating a new GiftCard
/// </summary>
public class GiftCardCreateValidator : AbstractValidator<GiftCardCreateCommand>
{
    private readonly IGiftCardService _service;

    /// <inheritdoc />
    public GiftCardCreateValidator(IGiftCardService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Number)
            .NotEmpty()
            .Length(y => (y.Entity.GiftCardType == 0) ? 12 : 9, y => (y.Entity.GiftCardType == 0) ? 12 : 9)
            .WithMessage(y => $"Gift Card number must have {((y.Entity.GiftCardType == 0) ? 12 : 9)} digits.")
            .MustAsync(NumberUnique).WithMessage("Gift Card number already exists.");
    }

    private async Task<bool> NumberUnique(string number, CancellationToken cancellationToken)
    {
        var upcExist = await _service.GetByCardNumber(number);
        return upcExist == null;
    }
}

/// <summary>
///    Handler for creating a new GiftCard
/// </summary>
public class GiftCardCreateHandler : IRequestHandler<GiftCardCreateCommand, GiftCardCreateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IGiftCardService _service;
    private readonly IAuthenticateUser<string> _auth;
    private readonly IBOUserService _users;

    /// <summary>
    ///    Constructor for inject services
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="auth"></param>
    /// <param name="users"></param>
    /// <param name="mapper"></param>
    public GiftCardCreateHandler(
        ILogger<GiftCardCreateHandler> logger,
        IGiftCardService service,
        IAuthenticateUser<string> auth,
        IBOUserService users,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        this._auth = auth;
        this._users = users;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<GiftCardCreateResponse> Handle(GiftCardCreateCommand request, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<GiftCard>(request.Entity);
        entity.DateSold = DateTime.Now;
        var rUser = await _users.GetByUserId(_auth.UserId);

        if (rUser == null)
        {
            throw new BusinessLogicException("User not found.");
        }

        entity.EmployeeId = rUser.Id;
        entity.EmployeeName = rUser.UserName;
        var result = await _service.Post(entity);
        _logger.LogInformation("Create Gift Card {ResultNumber} for user {ResultUserCreatorId}", result.Number,
            result.UserCreatorId);
        return new GiftCardCreateResponse { Data = _mapper.Map<GiftCardModel>(result) };
    }
}

/// <summary>
///   Response after creating a new GiftCard
/// </summary>
public record GiftCardCreateResponse : CQRSResponse<GiftCardModel>;