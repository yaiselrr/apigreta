using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class ScaleHomeFavModel : IDtoLong<string>, IMapFrom<ScaleHomeFav>
    {
        public long DepartmentId { get; set; }

        public long StoreId { get; set; }

        public DepartmentModel Department { get; set; }

        public StoreModel Store { get; set; }

        public List<long> ScaleProductIds { get; set; }

        public List<ScaleProductModel> ScaleProducts { get; set; }

        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ScaleHomeFav, ScaleHomeFavModel>().ReverseMap()
                .ForMember(vm => vm.ScaleProducts,
                    m => m.MapFrom(u => u.ScaleProductIds.Select(x => new ScaleProductModel {Id = x})));
        }
    }
}