namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class BaseSearchModel
    {
        // public bool LoadChildren { get; set; }
        public bool IsPagingEnabled { get; set; } = true;

        /// <summary>
        ///     Sort data ( field_direction ex name_asc or description_desc )
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        ///     Basic search string
        /// </summary>
        public string Search { get; set; }
    }
}