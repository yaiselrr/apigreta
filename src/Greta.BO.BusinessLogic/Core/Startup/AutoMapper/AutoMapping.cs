using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Greta.Sdk.Core.Models.Pager;

namespace Greta.BO.BusinessLogic.Core.Startup.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            var dlls = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName != null && x.FullName.Contains("Greta")).ToList();
            foreach (var a in dlls) ApplyMappingsFromAssembly(a);

            CreateMap(typeof(Pager<>), typeof(Pager<>));
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod("Mapping")
                                 ?? type.GetInterface("IMapFrom`1")?.GetMethod("Mapping");

                methodInfo?.Invoke(instance, new object[] {this});
            }
        }
    }
}