using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities.Dto
{
    public class SendOnlineStoreDataDto
    {
        public string FilePath { get; set; }
        public long TaskJob { get; set; }
        /// <summary>
        /// Store the int representation for PLU department or category
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Store the int representation for action Download or Upload for now only Download whos used
        /// </summary>
        public int  Action { get; set; }

        public LocationServerType LocationServerType { get; set; }
        public string Name { get; set; }
        public string NameWebsite { get; set; }
    }
}