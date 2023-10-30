using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class ScaleLabelDefinition : BaseEntityLong, IFullSyncronizable
    {
        public long ScaleProductId { get; set; }
        public long ScaleLabelType1Id { get; set; }
        public long? ScaleLabelType2Id { get; set; }
        public long ScaleBrandId { get; set; }

        public virtual ScaleProduct ScaleProduct { get; set; }
        public virtual ScaleLabelType ScaleLabelType1 { get; set; }
        public virtual ScaleLabelType ScaleLabelType2 { get; set; }
        
        
        #region Import Stuff

        public int ImportLabel { get; set; }
        
        #endregion
    }
}