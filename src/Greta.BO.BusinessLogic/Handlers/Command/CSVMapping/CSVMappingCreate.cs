using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Greta.BO.BusinessLogic.Handlers.Command.CSVMapping
{
    public static class CSVMappingCreate
    {
        public record Command(CSVMappingModel entity) : IRequest<Response>//, IAuthorizable
        {
            // public List<IRequirement> Requirements => new()
            // {
            //     new PermissionRequirement.Requirement($"add_edit_csv_mapping")
            // };
        }

        public class Validator : AbstractValidator<Command>
        {
            private readonly ICSVMappingService _service;

            public Validator(ICSVMappingService service)
            {
                // _service = service;
                // RuleFor(x => x.entity.ModelHeader)
                //     .NotEmpty();

                //RuleFor(x => x.entity.ModelImport)
                //    .NotEmpty();
                //
                // RuleFor(x => x.entity.CsvHeader)
                //     .NotEmpty();
                //
                // RuleFor(x => x.entity.Name)
                //     .NotEmpty()
                //     .MustAsync(NameUnique).WithMessage("Name already exists.");
                //
                // RuleFor(x => x.entity)
                //     .Must(SameLength).WithMessage("Model headers count isn't equals to CSV headers count.");
            }

            private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
            {
                var upcExist = await _service.GetByName(name);
                return upcExist == null;
            }

            private bool SameLength(CSVMappingModel entity)
            {
                return entity.CsvHeader.Count == entity.ModelHeader.Count;
            }
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly ICSVMappingService _service;

            public Handler(
                ILogger<Handler> logger,
                ICSVMappingService service,
                IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var errors = new List<string>();

                var mapping = _mapper.Map<Api.Entities.CSVMapping>(request.entity);

                //create mapp dictionary
                var mapper = new Dictionary<string, string>();
                for (var i = 0; i < request.entity.ModelHeader.Count; i++)
                    mapper.Add(request.entity.CsvHeader[i], request.entity.ModelHeader[i]);
                //validate required columns
                switch (request.entity.ModelImport)
                {
                    case ModelImport.PRODUCT:
                        if (!mapper.Values.Any(x => x == nameof(Api.Entities.Product.Name)))
                            errors.Add($"You must select a header for {nameof(Api.Entities.Product.Name)} column.");
                        if (!mapper.Values.Any(x => x == nameof(Api.Entities.Product.DepartmentId)))
                            errors.Add(
                                $"You must select a header for {nameof(Api.Entities.Product.DepartmentId)} column.");
                        if (!mapper.Values.Any(x => x == nameof(Api.Entities.Product.CategoryId)))
                            errors.Add(
                                $"You must select a header for {nameof(Api.Entities.Product.CategoryId)} column.");
                        if (!mapper.Values.Any(x => x == nameof(Api.Entities.Product.UPC)))
                            errors.Add($"You must select a header for {nameof(Api.Entities.Product.UPC)} column.");
                        break;
                    case ModelImport.SCALE_PRODUCT:
                        if (!mapper.Values.Any(x => x == nameof(ScaleProduct.Name)))
                            errors.Add($"You must select a header for {nameof(ScaleProduct.Name)} column.");
                        if (!mapper.Values.Any(x => x == nameof(ScaleProduct.DepartmentId)))
                            errors.Add($"You must select a header for {nameof(ScaleProduct.DepartmentId)} column.");
                        if (!mapper.Values.Any(x => x == nameof(ScaleProduct.CategoryId)))
                            errors.Add($"You must select a header for {nameof(ScaleProduct.CategoryId)} column.");
                        if (!mapper.Values.Any(x => x == nameof(ScaleProduct.UPC)))
                            errors.Add($"You must select a header for {nameof(ScaleProduct.UPC)} column.");
                        break;
                    case ModelImport.DEPARTMENT:
                        if (!mapper.Values.Any(x => x == nameof(Api.Entities.Department.Name)))
                            errors.Add($"You must select a header for {nameof(Api.Entities.Department.Name)} column.");
                        if (!mapper.Values.Any(x => x == nameof(Api.Entities.Department.DepartmentId)))
                            errors.Add(
                                $"You must select a header for {nameof(Api.Entities.Department.DepartmentId)} column.");
                        break;
                    case ModelImport.CATEGORY:
                        if (!mapper.Values.Any(x => x == nameof(Api.Entities.Category.Name)))
                            errors.Add($"You must select a header for {nameof(Api.Entities.Category.Name)} column.");
                        if (!mapper.Values.Any(x => x == nameof(Api.Entities.Category.DepartmentId)))
                            errors.Add(
                                $"You must select a header for {nameof(Api.Entities.Category.DepartmentId)} column.");
                        if (!mapper.Values.Any(x => x == nameof(Api.Entities.Category.CategoryId)))
                            errors.Add(
                                $"You must select a header for {nameof(Api.Entities.Category.CategoryId)} column.");
                        break;
                    case ModelImport.SCALE_CATEGORY:
                        if (!mapper.Values.Any(x => x == nameof(Api.Entities.ScaleCategory.Name)))
                            errors.Add(
                                $"You must select a header for {nameof(Api.Entities.ScaleCategory.Name)} column.");
                        if (!mapper.Values.Any(x => x == nameof(Api.Entities.ScaleCategory.DepartmentId)))
                            errors.Add(
                                $"You must select a header for {nameof(Api.Entities.ScaleCategory.DepartmentId)} column.");
                        if (!mapper.Values.Any(x => x == nameof(Api.Entities.ScaleCategory.CategoryId)))
                            errors.Add(
                                $"You must select a header for {nameof(Api.Entities.ScaleCategory.CategoryId)} column.");
                        break;
                }

                //create mapping if columns required exist
                if (!errors.Any())
                {
                    //serialize mmapper
                    mapping.MapperJson = JsonConvert.SerializeObject(mapper);

                    //creating mapping
                    var result = await _service.Post(mapping);
                    _logger.LogInformation($"Create CSVMapping {result.Id} for user {result.UserCreatorId}");
                    return new Response {Data = _mapper.Map<CSVMappingModel>(result)};
                }

                return new Response {Errors = errors, StatusCode = HttpStatusCode.BadRequest};
            }
        }

        public record Response : CQRSResponse<CSVMappingModel>;
    }
}