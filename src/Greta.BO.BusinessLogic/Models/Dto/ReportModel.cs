using System;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class ReportModel : IDtoLong<string>, IMapFrom<Api.Entities.Report>
    {
        public long Id { get; set; }

       
        public string Name { get; set; }
        
        public string Path { get; set; }

        public Guid? GuidId { get; set; }

        public ReportCategory Category { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }        
    }
}
