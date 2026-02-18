using AutoMapper;
using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Identity.DataAndons.Dtos;
using MiniSkeletonAPI.Application.Identity.MRepairs.Dtos;
using MiniSkeletonAPI.Application.Identity.MWarnas.Dtos;
using MiniSkeletonAPI.Application.Identity.Parts.Dtos;
using MiniSkeletonAPI.Application.Identity.Roles.Dtos;
using MiniSkeletonAPI.Application.Identity.Settings.Dtos;
using MiniSkeletonAPI.Application.Identity.Users.Queries.GetUsersWithPagination;
using MiniSkeletonAPI.Domain.Entities;
using MiniSkeletonAPI.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Infrastructure.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
            CreateMap<ApplicationUser, UserBriefDto>();
            CreateMap<ApplicationRole, RoleBriefDto>(); 
            CreateMap<Part, PartBriefDto>();
            CreateMap<DataCounting, SettingSpeedBriefDto>();
            //CreateMap<DataAndon, DataAndonBriefDto>();
            CreateMap<DataAndon, DataAndonBriefDto>()
                .ForMember(dest => dest.estimatedTime,
                    opt => opt.MapFrom(src =>
                        src.StarProsess.HasValue
                            ? src.StarProsess.Value.AddMinutes(
                                ((double)(466) / ((double)(src.HangerSpeed ?? 1))) 
                                + ((double)(src.stopLine ?? 0)) 
                            )
                            : (DateTime?)null
                    ));






            CreateMap<MWarna,MWarnaBriefDto>();
            CreateMap<MRepair,MRepairBriefDto>();
            CreateMap<Setting, SettingBriefDto>();

            //CreateMap(DataAndonDetail)



        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var mapFromType = typeof(IMapFrom<>);

            var mappingMethodName = nameof(IMapFrom<object>.Mapping);

            bool HasInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;

            var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(HasInterface)).ToList();

            var argumentTypes = new Type[] { typeof(Profile) };

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod(mappingMethodName);

                if (methodInfo != null)
                {
                    methodInfo.Invoke(instance, new object[] { this });
                }
                else
                {
                    var interfaces = type.GetInterfaces().Where(HasInterface).ToList();

                    if (interfaces.Count > 0)
                    {
                        foreach (var @interface in interfaces)
                        {
                            var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);

                            interfaceMethodInfo.Invoke(instance, new object[] { this });
                        }
                    }
                }
            }
        }
    }
}
