namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class ProcessEndOfDayRequest
    {
        public EndOfDayHolder Holder { get; set; }
        public long StoreId { get; set; }
        public long ElementId { get; set; }
        public string Date { get; set; }
        public int Persist { get; set; }
    }

    public record EndOfDayHolder(
        int StartingCash, int Count100, int Count50, int Count20, int Count10, int Count5, int Count1,
        int Countc100, int Countc50, int Countc25, int Countc10, int Countc5, int Countc1);
}