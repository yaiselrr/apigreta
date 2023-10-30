using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class ProfilesModel : IMapFrom<Profiles>
    {
        [Required] public string Name { get; set; }

        [Required] public long ApplicationId { get; set; }

        public virtual ClientApplicationModel Application { get; set; }

        public virtual List<long> PermissionsIds { get; set; }
        public virtual List<PermissionModel> Permissions { get; set; }

        //public virtual List<FunctionGroup> FunctionGroups { get; set; }

        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Profiles, ProfilesModel>().ReverseMap()
                .ForMember(vm => vm.Permissions,
                    m => m.MapFrom(u => u.PermissionsIds
                        .Select(x => new PermissionModel {Id = x.ToString()})));
        }
    }
}