using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.AdBatch
{
    public static class AdBatchCreate
    {
        public record Command(AdBatchCreateModel entity) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"add_edit_ad_batch")
            };
        }

        public class Validator : AbstractValidator<Command>
        {
            private readonly IAdBatchService _service;

            public Validator(IAdBatchService service)
            {
                _service = service;
                RuleFor(x => x.entity.Name)
                    .NotEmpty()
                    .Length(3, 64)
                    .MustAsync(NameUnique).WithMessage("AdBatch name already exists.");
            }

            private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
            {
                var upcExist = await _service.GetByName(name);
                return upcExist == null;
            }
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IAdBatchService _service;
            protected readonly IStoreService _serviceStore;

            public Handler(
                ILogger<Handler> logger,
                IAdBatchService service,
                IStoreService serviceStore,
                IMapper mapper)
            {
                _logger = logger;
                _serviceStore = serviceStore;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Api.Entities.AdBatch>(request.entity);
                entity.Type = RetailPriceBatchType.AD;
                if (request.entity.AllStores)
                {
                    var stores = await _serviceStore.GetAllIds();
                    // var result = await _service.CreateOnMultipleStores(stores, entity);
                    entity.Stores = stores.Select(x => new Api.Entities.Store() {Id = x}).ToList();
                    var result = await _service.Post(entity);
                    request.entity.Id = result.Id;
                    _logger.LogInformation($"Create AdBatch {result.Name} for user {result.UserCreatorId}");
                    return new Response {Data = request.entity};
                }

                if (request.entity.RegionId > 0)
                {
                    var stores = await _serviceStore.GetByRegion(request.entity.RegionId);
                    entity.Stores = stores.Select(x => new Api.Entities.Store() {Id = x}).ToList();
                    var result = await _service.Post(entity);
                    request.entity.Id = result.Id;
                    _logger.LogInformation($"Create AdBatch {result.Name} for user {result.UserCreatorId}");
                    return new Response {Data = request.entity};
                }
                else
                {
                    entity.Stores = new List<Api.Entities.Store>(){new Api.Entities.Store(){ Id = request.entity.StoreId}};
                    var result = await _service.Post(entity);
                    request.entity.Id = result.Id;
                    _logger.LogInformation($"Create AdBatch {result.Name} for user {result.UserCreatorId}");
                    return new Response {Data = request.entity};
                }
            }
        }

        public record Response : CQRSResponse<AdBatchCreateModel>;
    }
}