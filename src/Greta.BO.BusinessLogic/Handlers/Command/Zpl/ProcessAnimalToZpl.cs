using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.AnimalSpecs;
using Greta.Sdk.LabelConverter;
using Greta.Sdk.LabelConverter.models;
using MediatR;
using Newtonsoft.Json;

namespace Greta.BO.BusinessLogic.Handlers.Command.Zpl;

public record ProcessAnimalToZplCommand(long AnimalId, long TagId) : IRequest<ProcessAnimalToZplResponse>;

/// <summary>
/// Process a product to zpl response
/// </summary>
public record ProcessAnimalToZplResponse : CQRSResponse<string>;

/// <inheritdoc />
public class ProcessAnimalToZplHandler : IRequestHandler<ProcessAnimalToZplCommand, ProcessAnimalToZplResponse>
{
    private readonly IMediator _mediator;
    private readonly IAnimalService _animalService;
    private readonly IScaleLabelTypeService _scaleLabelTypeService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scaleLabelTypeService"></param>
    /// <param name="animalService"></param>
    /// <param name="mediator"></param>
    public ProcessAnimalToZplHandler(IScaleLabelTypeService scaleLabelTypeService, IAnimalService animalService, IMediator mediator)
    {
        _scaleLabelTypeService = scaleLabelTypeService;
        _animalService = animalService;
        _mediator = mediator;
    }
    
    
    public async  Task<ProcessAnimalToZplResponse> Handle(ProcessAnimalToZplCommand request, CancellationToken cancellationToken)
    {
        var labelType = await _scaleLabelTypeService.Get(request.TagId);
        var label = JsonConvert.DeserializeObject<LabelDesign>(labelType.Design);
        var specs = new AnimalGetByIdWithChildrenSpecs(request.AnimalId);
        var animal = await _animalService.Get(specs, cancellationToken);
        
        //prepare customer data in format customername,customer2name, customer3name
        var customers = string.Join(",", animal.Customers.Select(x => x.FirstName).ToList());
        
        //prepare upc with rancher and tag number
        if (!int.TryParse(animal.Tag, out int value))
        {
            throw new BussinessValidationException("Tag is not a integer.");
        }
        var upc = $"{animal.RancherId:D6}0{value:D6}";

        var model = new AnimalHolderModel
        {
            UPC = upc,
            Rancher = animal.Rancher.Name,
            Tag = animal.Tag,
            Breed = animal.Breed.Name,
            Customer = customers,
            DateReceived = animal.DateReceived?.ToString("MM/dd/yyyy"),
            DateSlaughtered = animal.DateSlaughtered?.ToString("MM/dd/yyyy"),
            LiveWeight = animal.LiveWeight.ToString(),
            RailWeight = animal.RailWeight.ToString(),
            SubPrimalWeight = animal.SubPrimalWeight.ToString(),
            CutWeight = animal.CutWeight.ToString(),
            Store = animal.Store.Name
        };

        return new ProcessAnimalToZplResponse() { Data = model.ToZpl(label) };
    }
}