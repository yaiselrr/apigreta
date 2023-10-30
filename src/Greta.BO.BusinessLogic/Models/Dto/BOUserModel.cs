using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Identity.Api.EventContracts.BO.User;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class BOUserModel : IMapFrom<BOUser>, IDtoLong<string>
    {
        public long? BOProfileId;
        public long? POSProfileId;

        public BOUserModel()
        {
        }

        public BOUserModel(BOUser userBO, UserGetInfoResponseContract userIdentity)
        {
            if (userBO != null)
            {
                BOProfileId = userBO.BOProfileId;
                BOProfile = userBO.BOProfile;
                CreatedAt = userBO.CreatedAt;
                Id = userBO.Id;
                Pin = userBO.Pin;
                POSProfileId = userBO.POSProfileId;
                POSProfile = userBO.POSProfile;
                RoleId = userBO.RoleId;
                State = userBO.State;
                UpdatedAt = userBO.UpdatedAt;
                UserCreatorId = userBO.UserCreatorId;
                UserId = userBO.UserId;
                Stores = userBO.Stores;
                FirstName = userIdentity.FirstName;
                LastName = userIdentity.LastName;
            }

            if (userIdentity != null)
            {
                Email = userIdentity.Email;
                EmailConfirmed = userIdentity.EmailConfirmed;
                PhoneNumberConfirmed = userIdentity.PhoneNumberConfirmed;
                PhoneNumber = userIdentity.PhoneNumber;
                UserName = userIdentity.UserName;
                LockoutEnabled = userIdentity.LockoutEnabled;
                CanCreateUsers = userIdentity.CanCreateUsers;
                TwoFactorEnabled = userIdentity.TwoFactorEnabled;
            }
        }

        public Profiles BOProfile { get; set; }
        public Profiles POSProfile { get; set; }
        [Required] public long? RoleId { get; set; }

        // public long? BOProfileId
        // {
        //     get => boProfileId == null ? -1 : boProfileId;
        //     set => boProfileId = value;
        // }

        // public long? POSProfileId
        // {
        //     get => posProfileId == null ? -1 : posProfileId;
        //     set => posProfileId = value;
        // }

        public string UserId { get; set; }

        public string Pin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required] public string UserName { get; set; }

        [Required] [EmailAddress] public string Email { get; set; }

        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public bool CanCreateUsers { get; set; }
        public bool TwoFactorEnabled { get; set; }
        
        public List<long> StoresIds { get; set; }
        
        public List<Store> Stores { get; set; }

        public long Id { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<BOUser, BOUserModel>().ReverseMap()
                .ForMember(vm => vm.Stores, m => m.MapFrom(u => u.StoresIds.Select(x => new Store {Id = x})));
        }
    }
}