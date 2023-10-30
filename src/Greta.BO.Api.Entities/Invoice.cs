using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities
{
    internal class Invoice : BaseEntityLong
    {
        public InvoiceStatus InvoiceStatus { get; set; }
    }
}