
using AutoMapper;
using MiniSkeletonAPI.Application.Identity.MWarnas.Dtos;


namespace MiniSkeletonAPI.Infrastructure.Identity
{
    public class ApplicationMWarna
    {
        public ApplicationMWarna(string coler, string description)
        {

            Coler = coler;
            Description = description;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Coler { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }

    }
    public class MWarnaMappingProfile : Profile
    {
        public MWarnaMappingProfile()
        {
            CreateMap<ApplicationMWarna, MWarnaBriefDto>().ReverseMap();
        }
    }
}
