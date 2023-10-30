namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class GetEndOfDayresumeRequest
    {
        public string InitialDate { get; set; }
        
        public string EndDate { get; set; }
        
        public long StoreId { get; set; }
    }
}