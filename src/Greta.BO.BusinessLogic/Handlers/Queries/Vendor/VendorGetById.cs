using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Vendor
{
    public static class VendorGetById
    {
        public record Query(long Id) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new() { 
                new PermissionRequirement.Requirement($"view_tender_type")
            };
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            protected readonly IImageService _imageService;
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IVendorService _service;

            public Handler(ILogger<Handler> logger, IVendorService service, IImageService imageService, IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _imageService = imageService;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _service.GetWithImages(request.Id);

                var data = _mapper.Map<VendorModel>(entity.Vendor);

                data.VendorContacts = entity.VendorContactImages.Map(x =>
                {
                    var res = _mapper.Map<VendorContactModel>(x.VendorContact);
                    res.Image = _mapper.Map<ImageModel>(x.Image);
                    return res;
                }).ToList();
                return data == null ? null : new Response { Data = data };
            }
        }

        public record Response : CQRSResponse<VendorModel>;
    }
}