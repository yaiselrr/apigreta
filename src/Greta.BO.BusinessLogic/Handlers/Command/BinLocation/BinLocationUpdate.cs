using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.BinLocation
{
    public class BinLocationUpdate
    {
        public record Command(long Id, BinLocationModel entity) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"add_edit_bin_location")
            };
        }

        public class Validator : AbstractValidator<Command>
        {
            private readonly IBinLocationService _service;

            public Validator(IBinLocationService service)
            {
                _service = service;
                /*
                 RuleFor(x => x.entity.Name)
                    .NotEmpty()
                    .Length(3, 64);
                */

                RuleFor(x => x.entity.Store).GreaterThan(0).WithMessage("Store must be selected");
                RuleFor(x => x.entity.Side).GreaterThanOrEqualTo(0).WithMessage("Side must be selected");
                
                RuleFor(x => x.entity)
                    .MustAsync(NameStoreUnique).WithMessage("BinLocation already exists for this store.");
            }

            private async Task<bool> NameStoreUnique(Command comand, BinLocationModel entity, CancellationToken cancellationToken)
            {
                var binLocations = await _service.Get();
                bool res = !(binLocations.Any(x => x.Name == entity.Name && x.Store == entity.Store && x.Id != comand.Id));
                return res;
            }
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IBinLocationService _service;

            public Handler(
                ILogger<Handler> logger,
                IBinLocationService service,
                IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Api.Entities.BinLocation>(request.entity);
                var success = await _service.Put(request.Id, entity);
                _logger.LogInformation($"Bin Location {request.Id} update successfully.");
                return new Response {Data = success};
            }
        }

        public record Response : CQRSResponse<bool>;
    }
}