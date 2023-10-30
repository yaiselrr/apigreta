namespace Greta.BO.Api.Entities
{
    public class BatchClose: BaseEntityLong
    {
        public long DeviceId { get; set; }
        public Device Device{ get; set; }
        public string AcquirerName { get; set; }
        public decimal EBTAmount { get; set; }
        public string Batch { get; set; }
        public int BatchRecordCount { get; set; }
        public string HostResponseText { get; set; }
        public decimal HostTotalsAmount1 { get; set; }
        public decimal HostTotalsAmount5 { get; set; }
        public int HostTotalsCount1 { get; set; }
        public int HostTotalsCount5 { get; set; }
        public string MerchantName { get; set; }
        public string TerminalNumber { get; set; }
        public string TransactionResponse { get; set; }
    }
    
    public class BatchCloseDto
    {
        public long DeviceId { get; set; }
        public Device Device{ get; set; }
        public string AcquirerName { get; set; }
        public decimal EBTAmount { get; set; }
        public string Batch { get; set; }
        public string BatchRecordCount { get; set; }
        public string HostResponseText { get; set; }
        public string HostTotalsAmount1 { get; set; }
        public string HostTotalsAmount5 { get; set; }
        public string HostTotalsCount1 { get; set; }
        public string HostTotalsCount5 { get; set; }
        public string MerchantName { get; set; }
        public string TerminalNumber { get; set; }
        public string TransactionResponse { get; set; }
    }
}