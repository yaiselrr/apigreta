using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Interfaces;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class ScaleLabelType : BaseEntityLong, IFullSyncronizable, INameUniqueEntity
    {
        public string Name { get; set; }

        /// <summary>
        ///     This value if ScaleType is GretaLabel then this is 500+
        ///     if ScaleType is External then this value is 1 to 250
        /// </summary>
        /// <value></value>
        public int LabelId { get; set; }

        public ScaleType ScaleType { get; set; }

        // public long ScaleLabelDesignId { get; set; }
        // public ScaleLabelDesign ScaleLabelDesign { get; set; }
        public string Design { get; set; }
    }
}