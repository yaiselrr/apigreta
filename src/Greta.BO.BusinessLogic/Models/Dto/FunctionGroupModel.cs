using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class FunctionGroupModel : IMapFrom<FunctionGroup>, IDtoLong<string>
    {
        public string Name { get; set; }
        public virtual List<PermissionModel> Permissions { get; set; }

        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}