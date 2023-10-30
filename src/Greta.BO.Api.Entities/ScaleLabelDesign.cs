namespace Greta.BO.Api.Entities
{
    public class ScaleLabelDesign: BaseEntityLong
    {
        public long ScaleLabelTypeId { get; set; }
        public ScaleLabelType ScaleLabelType { get; set; }
        public string Design { get; set; }
    }
}