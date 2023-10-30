namespace Greta.BO.BusinessLogic.Models.Dto
{
    /// <summary>
    /// Model to change target gross profit to products
    /// </summary>
    public class CategoryTargetGrossProfitModel 
    {
        /// <summary>
        /// Store id, if is not selected => return -1
        /// </summary>
        public int StoreId { get; set; }
        
        /// <summary>
        /// Region id, if is not selected => return -1
        /// </summary>
        public int RegionId { get; set; }
        
        /// <summary>
        /// Return if gross profit change all products of all stores
        /// </summary>
        public bool AllStores { get; set; }
        
        /// <summary>
        /// New gross profit
        /// </summary>
        public decimal TargetGrossProfit { get; set; }
    }
}