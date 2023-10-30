using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.Sdk.LabelConverter;
using Greta.Sdk.LabelConverter.models;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Command.Zpl;


/// <summary>
/// Convert a label design to zpl code with example data
/// </summary>
/// <param name="Label">Label Design</param>
/// <param name="Type">Type of data</param>
public record ConvertZplForExampleCommand(LabelDesign Label, LabelDesignMode Type): IRequest<ConvertZplForExampleResponse>;
/// <summary>
/// Convert a label design to zpl code with example data response
/// </summary>
public record ConvertZplForExampleResponse : CQRSResponse<string>;
/// <summary>
/// Convert a label design to zpl code with example data handler
/// </summary>
public class ConvertZplForExampleHandler:IRequestHandler<ConvertZplForExampleCommand, ConvertZplForExampleResponse>
{
    /// <inheritdoc />
    public Task<ConvertZplForExampleResponse> Handle(ConvertZplForExampleCommand request, CancellationToken cancellationToken = default)
    {
        
        switch(request.Type)
        {
            case LabelDesignMode.ShelTag:
                return Task.FromResult(new ConvertZplForExampleResponse() { Data = GetShelfTagDemo(request).ToZpl(request.Label) });
            case LabelDesignMode.Animal:
                return Task.FromResult(new ConvertZplForExampleResponse() { Data = GetAnimalDemo(request).ToZpl(request.Label) });
            case LabelDesignMode.Label:
            default:
                return Task.FromResult(new ConvertZplForExampleResponse() { Data = GetLabelDemo(request).ToZpl(request.Label) });
        }
    }

    private static HolderModel GetLabelDemo(ConvertZplForExampleCommand request)
    {
        var upc = "1234567891234";
        var haveEan13 = request.Label.items.Any(x => x.type == "barcodeTool"/* && x.format == "EAN13"*/);//Add data to use other barcode types
        if (haveEan13)
        {
            upc = "00234";
        }
        return new HolderModel()
        {
            UPC = upc,
            ByCount = "1",
            NetWeight = "1.1",
            RetailPrice = "1.11",
            TotalPrice = "2.00",
            Cholesterol = "1",
            CholesterolP = "1",
            Sodium = "1",
            SodiumP = "1",
            TotalCarbohydrate = "1",
            TotalCarbohydrateP = "1",
            DietaryFiber = "1",
            DietaryFiberP = "1",
            SaturatedFatP = "1",
            TotalSugar = "1",
            AddedSugarP = "1",
            Protein = "1",
            VitD = "1",
            VitDP = "1",
            Calcium = "1",
            CalciumP = "1",
            Iron = "1",
            IronP = "1",
            AddedSugar = "1",
            Potas = "1",
            SaturatedFatGrams = "1",
            TotalFat = "1",
            //UPC = Product.UPC,
            //RetailPrice = formatconv.Convert(Price, null, null, null).ToString(),
            //TotalPrice = formatconv.Convert(TotalPrice, null, null, null).ToString(),
            Description1 = "Product name",
            Description2 = "Product name2",
            Description3 = "Product name3",
            PackageWeight = "1",
            //NetWeight = NetWeight.ToString(),
            TotalFatP = "1",
            //ByCount = ByCount.ToString(),
            ProduceLife = "1",
            Text1 = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
            Text2 = "Text2",
            Text3 = "Text3",
            Text4 = "Text4",
            ServingsContainer = "1",
            ServingSize = "1",
            AmountServingCalories = "1",
            ShelfLife = "1",
            PotasP = "1",
        };
    }
    
    private static ShelfTagHolderModel GetShelfTagDemo(ConvertZplForExampleCommand request)
    {
        var upc = "1234567891234";
        var haveEan13 = request.Label.items.Any(x => x.type == "barcodeTool" && x.format == "EAN13");//Add data to use other barcode types
        if (haveEan13)
        {
            upc = "00234";
        }
        return new ShelfTagHolderModel()
        {
            Date = DateTime.Now.ToString("MM/dd/yyyy"),
            UPC = upc,
            RetailPrice = 99.99M.ToString(CultureInfo.CurrentCulture),
            VendorProductCode = "12345",
            ProductName = "Example Product",
            VendorName = "Vendor example",
            CategoryName = "Category Example",
            DepartmentName = "Department Example",
            VendorCasePack = "12",
            CashDiscountServiceFee = 99.99M.ToString(CultureInfo.CurrentCulture),
        };
    }
    
    private static AnimalHolderModel GetAnimalDemo(ConvertZplForExampleCommand request)
    {
        return new AnimalHolderModel
        {
            UPC = "0000010125436",
            Rancher = "1",
            Tag = "125436",
            Breed = "1",
            Customer = "John Doe",
            DateReceived = DateTime.Now.ToString("MM/dd/yyyy"),
            DateSlaughtered = DateTime.Now.ToString("MM/dd/yyyy"),
            LiveWeight = 99.99M.ToString(CultureInfo.CurrentCulture),
            RailWeight = 99.99M.ToString(CultureInfo.CurrentCulture),
            SubPrimalWeight = 99.99M.ToString(CultureInfo.CurrentCulture),
            CutWeight = 99.99M.ToString(CultureInfo.CurrentCulture),
            Store = "The new moon"
        };
    }
}