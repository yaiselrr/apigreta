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
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.BinLocation
{
    public class BinLocationDelete
    {
        public record Command(long Id) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"delete_bin_location")
            };
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IBinLocationService _service;

            public class Validator : AbstractValidator<Command>
            {
                private readonly IBinLocationService _service;

                public Validator(IBinLocationService service)
                {
                    _service = service;

                    RuleFor(x => x.Id)
                        .MustAsync(CanDeleted)
                        .WithMessage($"This bin location cannot be deleted because it is associated with another element.");
                }

                private async Task<bool> CanDeleted(long id, CancellationToken cancellationToken)
                {
                    return await _service.CanDeleted(id);
                }
            }

            public Handler(ILogger<Handler> logger, IBinLocationService service, IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _service.CanDeleted(request.Id))
                {
                    throw new BusinessLogicException(
                        "This bin location cannot be deleted because it is associated with another element.");
                }
                var result = await _service.Delete(request.Id);
                _logger.LogInformation($"Entity with id {request.Id} Deleted successfully.");
                return new Response {Data = result};
            }
        }

        public record Response : CQRSResponse<bool>;
    }
}