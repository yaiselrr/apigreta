using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class ScaleReasonCodesSearchModel : BaseSearchModel, IMapFrom<ScaleReasonCodes>
    {
        public string Name { get; set; }
        public long DepartmentId { get; set; }
        public enum Type { PriceChange = 1, Shrink = 2 }
    }
}
