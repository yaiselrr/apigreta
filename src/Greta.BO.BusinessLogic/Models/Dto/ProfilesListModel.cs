using System;
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class ProfilesListModel : IMapFrom<Profiles>, IDtoLong<string>
    {
        public string Name { get; set; }
        public ClientApplicationModel Application { get; set; }

        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Profiles, ProfilesListModel>().ReverseMap();
        }
    }
}