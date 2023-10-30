using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class ImageModel : IMapFrom<Image>
    {
        public long Id { get; set; }
        public string Path { get; set; }
        public bool IsPrimary { get; set; }
    }
}