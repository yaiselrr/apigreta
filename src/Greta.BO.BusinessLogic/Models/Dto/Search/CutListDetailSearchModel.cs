using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    /// <inheritdoc />
    public class CutListDetailSearchModel: BaseSearchModel
    {
        /// <summary>
        /// Cut List Id
        /// </summary>
        public long CutListId { get; set; }
    }
}