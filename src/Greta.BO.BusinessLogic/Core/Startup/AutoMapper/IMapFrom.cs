using AutoMapper;

namespace Greta.BO.BusinessLogic.Core.Startup.AutoMapper
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile)
        {
            profile.CreateMap(typeof(T), GetType()).ReverseMap();
        }
    }
}