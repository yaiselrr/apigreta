using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using Greta.Corporate.Api.EventContracts.Store;
using Greta.Sdk.EFCore.Middleware;
using Greta.Sdk.MassTransit.Contracts;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Store;

/// <summary>
/// Update entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="Entity">New entity</param>
public record StoreUpdateCommand
    (long Id, StoreModel Entity) : IRequest<StoreUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Store).ToLower()}")
    };
}

/// <inheritdoc />
public class StoreUpdateValidator : AbstractValidator<StoreUpdateCommand>
{
    private readonly IStoreService _service;

    /// <inheritdoc />
    public StoreUpdateValidator(IStoreService service)
    {
        _service = service;

        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Store name already exists.");
    }

    private async Task<bool> NameUnique(StoreUpdateCommand command, string name, CancellationToken cancellationToken)
    {
        var nameExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Store>(name, command.Id), cancellationToken);
        return nameExist == null;
    }
}

/// <inheritdoc />
public class StoreUpdateHandler : IRequestHandler<StoreUpdateCommand, StoreUpdateResponse>
{
    protected readonly IAuthenticateUser<string> _authenticateUser;
    protected readonly IConfiguration _configuration;
    protected readonly ILogger _logger;
    protected readonly IMapper _mapper;
    protected readonly IStoreService _service;
    private readonly IRequestClient<UpdateStoreRequestContract> _client;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    /// <param name="client"></param>
    /// <param name="authenticateUser"></param>
    /// <param name="configuration"></param>
    public StoreUpdateHandler(
        ILogger<StoreUpdateHandler> logger,
                IStoreService service,
                IRequestClient<UpdateStoreRequestContract> client,
                IAuthenticateUser<string> authenticateUser,
                IConfiguration configuration,
                IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
        _client = client;
        _configuration = configuration;
        _authenticateUser = authenticateUser;
    }

    /// <inheritdoc />
    public async Task<StoreUpdateResponse> Handle(StoreUpdateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Guid storeGuid = (await _service.Get(request.Id)).GuidId;
            //update store on corporate
            var data = await _client.GetResponse<BooleanResponseContract, FailResponseContract>(new
            {
                __Header_user = _authenticateUser.UserId,
                State = true,
                request.Entity.Name,
                GuidId = storeGuid,
                BO_ClientCode = _configuration["Company:CompanyCode"],
            });

            if (data.Is(out Response<FailResponseContract> response))
            {
                _logger.LogError("Error update store in corporate");
                throw new BusinessLogicException(((List<string>)response.Message.ErrorMessages)[0]);
            }

            var entity = _mapper.Map<Api.Entities.Store>(request.Entity);
            entity.GuidId = storeGuid;
            var success = await _service.Put(request.Id, entity);
            _logger.LogInformation("Store {StoreId} update successfully", request.Id);
            return new StoreUpdateResponse { Data = success };
        }
        catch (Exception e)
        {
            throw new BusinessLogicException("Error update store " + e.Message + " " + e.InnerException?.Message);
        }
    }
}

/// <inheritdoc />
public record StoreUpdateResponse : CQRSResponse<bool>;