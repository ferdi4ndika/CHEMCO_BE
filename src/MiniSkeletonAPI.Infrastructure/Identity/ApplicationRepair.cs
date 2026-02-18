
using AutoMapper;
using MiniSkeletonAPI.Application.Identity.MRepairs.Dtos;


namespace MiniSkeletonAPI.Infrastructure.Identity
{
    public class ApplicationMRepair
    {
        public ApplicationMRepair(string repair)
        {

            Repair = repair;
       
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Repair { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }

    }
    public class MRepairMappingProfile : Profile
    {
        public MRepairMappingProfile()
        {
            CreateMap<ApplicationMRepair, MRepairBriefDto>().ReverseMap();
        }
    }
}
