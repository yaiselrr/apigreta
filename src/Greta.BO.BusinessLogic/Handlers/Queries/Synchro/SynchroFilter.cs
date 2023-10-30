using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.Core.Abstractions;
using Greta.Sdk.Core.Models.Pager;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Synchro
{
    public static class SynchroFilter
    {
        public record Query(int CurrentPage, int PageSize, SynchroSearchModel Filter) : IRequest<Response>;

        public class Handler : IRequestHandler<Query, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly ISynchroService _service;
            private readonly IStoreService _storeService;

            public Handler(ILogger<Handler> logger,
                ISynchroService service,
                IStoreService storeService,
                IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _storeService = storeService;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                //obtener todos los stores con sus synchros
                var stores = await _storeService.GetWithStores(
                    request.CurrentPage,
                    request.PageSize,
                    request.Filter.Search,
                    request.Filter.RegionId,
                    request.Filter.Status);

                var data = stores.Data.Map(store =>
                {
                    var syncroOpen = store.Synchros.Where(x => x.Status == SynchroStatus.OPEN).LastOrDefault();
                    var syncroClosed = store.Synchros.Where(x => x.Status == SynchroStatus.CLOSE).FirstOrDefault();

                    //Aqui es necesario cuando se incluya la informacion de las maquinas que se haga una query para tambien tener aqui 
                    //la informacion de cuantas maquinas estan actualizadas
                    var currentTag = store.Synchros.Where(x => x.Status == SynchroStatus.COMPLETE).LastOrDefault();

                    // var devices = currentTag == null
                    //     ? store.Devices
                    //     : store.Devices.Where(x => x.SynchroVersion != currentTag.Tag).ToList();
                    var devices = store.Devices;


                    return new StoreSynchroDto
                    {
                        Store = _mapper.Map<StoreInfoDto>(store),
                        Devices = _mapper.Map<List<SynchroFilter.DeviceDto>>(devices),
                        SynchroOpened = syncroOpen == null ? null : _mapper.Map<SynchroDto>(syncroOpen),
                        SynchroClosed = syncroClosed == null ? null : _mapper.Map<SynchroDto>(syncroClosed),
                        LastTag = currentTag?.Tag ?? -1
                    };
                }).ToList();

                return new Response
                {
                    Data = new Pager<StoreSynchroDto>(
                        stores.TotalItems,
                        data,
                        stores.CurrentPage,
                        stores.PageSize,
                        request.PageSize
                    )
                };
            }
        }

        public record Response : CQRSResponse<Pager<StoreSynchroDto>>;

        public class StoreSynchroDto : IDtoLong<string>
        {
            public StoreInfoDto Store { get; set; }

            public List<DeviceDto> Devices { get; set; }

            public SynchroDto SynchroOpened { get; set; }
            public SynchroDto SynchroClosed { get; set; }
            public SynchroDto LastClosed { get; set; }
            public long LastTag { get; set; }


            public long Id { get; set; }
            public bool State { get; set; }
            public string UserCreatorId { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
        }

        public class DeviceDto : IMapFrom<Api.Entities.Device>
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string DeviceId { get; set; }
            public Guid GuidId { get; set; }
            public int SynchroVersion { get; set; }
            public long StoreId { get; set; }
        }

        public class StoreInfoDto : IMapFrom<Api.Entities.Store>
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public int SynchroVersion { get; set; }
            public RegionInfoDto Region { get; set; }
            public bool Updated { get; set; }
        }

        public class RegionInfoDto : IMapFrom<Api.Entities.Region>
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }

        public class SynchroDto : IMapFrom<Api.Entities.Synchro>
        {
            public long Id { get; set; }
            public long Tag { get; set; }

            public SynchroStatus Status { get; set; }

            public string FilePath { get; set; }
        }
    }
}