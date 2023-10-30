using System;

namespace Greta.BO.Api.Entities;

/// <summary>
/// Store the removed products from sales
/// </summary>
public class SaleProductRemoved : BaseEntityLong
{
    public long? SaleId { get; set; }
    public Sale Sale { get; set; }

    public long ProductId { get; set; }
    public string  ProductName { get; set; }
    public string UPC { get; set; }

    public long EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public long EmployeeAcceptId { get; set; }
    public string EmployeeAcceptName { get; set; }

    public decimal Price { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal Quantity { get; set; }

    public DateTime RemoveTime { get; set; }

}