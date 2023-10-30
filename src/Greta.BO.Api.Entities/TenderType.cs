using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class TenderType : BaseEntityLong, IFullSyncronizable
    {
        public string Name { get; set; }

        /// <summary>
        ///     Determine if open drawer is needed
        /// </summary>
        public bool OpenDrawer { get; set; }
        /// <summary>
        /// Determine if this tender type use payment gateway
        /// </summary>
        public bool PaymentGateway { get; set; }
        public string DisplayAs { get; set; }
        public bool CashDiscount { get; set; }
    }
}