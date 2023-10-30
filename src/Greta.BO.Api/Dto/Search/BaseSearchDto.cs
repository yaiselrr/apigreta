namespace Greta.BO.Api.Dto.Search
{
    public class BaseSearchDto
    {
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