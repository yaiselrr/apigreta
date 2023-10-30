using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Hubs;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Handlers.DataHandlers;
using Greta.BO.BusinessLogic.Handlers.DataHandlers.Exporters;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Greta.BO.BusinessLogic.Handlers.Command.CSVMapping
{
    public static class CSVMappingImport
    {
        public record Command(CSVImportModel entity) : INotification//, IAuthorizable
        {
            // public List<IRequirement> Requirements => new()
            // {
            //     //new PermissionRequirement.Requirement($"add_edit_csv_mapping")
            // };
        } //<Response>;

        public class Validator : AbstractValidator<Command>
        {
            private readonly ICSVMappingService _service;

            public Validator(ICSVMappingService service)
            {
                _service = service;
                RuleFor(x => x.entity.CSVFile)
                    .NotEmpty();

                RuleFor(x => x.entity.Separator)
                    .NotEmpty();
            }
        }

        public class Handler : INotificationHandler<Command> //IRequestHandler<Command>//, Response>
        {
            private readonly IHubContext<FrontHub, IFrontHub> _hub;
            private readonly ILogger _logger;
            private readonly IMapper _mapper;
            private readonly ICSVMappingService _service;
            private readonly INotifier _notifier;

            public Handler(
                INotifier notifier,
                ILogger<Handler> logger,
                ICSVMappingService service,
                // ILogger<CategoryExport
                IMapper mapper)
            {
                _notifier = notifier;
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            //public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            public async Task Handle(Command request, CancellationToken cancellationToken)

            {
                await _notifier.NotifyUpdateAsync(HandlerMessage.New("Initializing importation"));
                var mapping = new Dictionary<string, string>();
                var modelImport = request.entity.ModelImport;

                if (request.entity.MappingId == -1) //not use mapping exist
                {
                    await _notifier.NotifyUpdateAsync(HandlerMessage.New($"Please wait reading mapping from request data"));
                    //create mapp dictionary
                    for (var i = 0; i < request.entity.ModelHeader.Count; i++)
                        mapping.Add(request.entity.CsvHeader[i], request.entity.ModelHeader[i]);
                }
                else
                {
                    await _notifier.NotifyUpdateAsync(HandlerMessage.New($"Please wait reading mapping from database"));
                    //get mapping and dictionary
                    var csvMapping = await _service.GetById(request.entity.MappingId);
                    modelImport = csvMapping.ModelImport;
                    mapping = JsonConvert.DeserializeObject<Dictionary<string, string>>(csvMapping.MapperJson);
                }

                await _service.CSVMappingImport(
                    request.entity.CSVFile,
                    request.entity.Separator,
                    modelImport,
                    request.entity.StoresId,
                    mapping);

                //await _notifier.NotifyUpdateAsync(HandlerMessage.New($"Import CSV File successfully"));
                _logger.LogInformation("Import CSV File successfully.");
            }
        }

        public record Response : CQRSResponse<object>;
    }
}
