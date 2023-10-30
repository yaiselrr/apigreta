using System.Collections.Generic;
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
    public class AdBatchUpdate
    {
        public record Command(long Id, AdBatchModel entity) : IRequest<Response>, IAuthorizable
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
                    .MustAsync(NameUnique).WithMessage("Region name already exists.");
            }

            private async Task<bool> NameUnique(Command comand, string name, CancellationToken cancellationToken)
            {
                var upcExist = await _service.GetByName(name, comand.Id);
                return upcExist == null;
            }
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IAdBatchService _service;

            public Handler(
                ILogger<Handler> logger,
                IAdBatchService service,
                IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Api.Entities.AdBatch>(request.entity);
                entity.Type = RetailPriceBatchType.AD;
                var success = await _service.Put(request.Id, entity);
                _logger.LogInformation($"AdBatch {request.Id} update successfully.");
                return new Response {Data = success};
            }
        }

        public record Response : CQRSResponse<bool>;
    }
}