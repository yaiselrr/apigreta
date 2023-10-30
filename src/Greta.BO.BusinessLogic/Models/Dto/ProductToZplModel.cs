using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.BusinessLogic.Models.Dto;

public class ProductToZplModel
{
    public int QtyToPrint { get; set; }
    public long StoreId { get; set; }
    public long? TagId { get; set; }
    public long ProductId { get; set; }
}

public class ToZplExmpleRequest
{
    public string Design { get; set; }

    public LabelDesignMode Type { get; set; }
}