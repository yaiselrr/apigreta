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

namespace Greta.BO.BusinessLogic.Handlers.Command.PriceBatch
{
    public class PriceBatchUpdate
    {
        public record Command(long Id, PriceBatchModel entity) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"add_edit_price_batch")
            };
        }

        public class Validator : AbstractValidator<Command>
        {
            private readonly IPriceBatchService _service;

            public Validator(IPriceBatchService service)
            {
                _service = service;
                RuleFor(x => x.entity.Name)
                    .NotEmpty()
                    .Length(3, 64)
                    .MustAsync(NameUnique).WithMessage("PriceBatch name already exists.");
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
            protected readonly IPriceBatchService _service;

            public Handler(
                ILogger<Handler> logger,
                IPriceBatchService service,
                IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Api.Entities.PriceBatch>(request.entity);
                entity.Type = RetailPriceBatchType.Batch;
                var success = await _service.Put(request.Id, entity);
                _logger.LogInformation($"PriceBatch {request.Id} update successfully.");
                return new Response {Data = success};
            }
        }

        public record Response : CQRSResponse<bool>;
    }
}