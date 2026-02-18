using AutoMapper;

using MiniSkeletonAPI.Application.Identity.Parts.Dtos;


namespace MiniSkeletonAPI.Infrastructure.Identity
{
    public class ApplicationPart
    {
        public ApplicationPart(string partName, string description, string partNumber, int qty)
        {

            PartName =partName ;
            Description = description;
            PartNumber = partNumber;
            Qty = qty;
            CreatedAt = DateTime.UtcNow;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string PartName { get; set; } = null!;
        public string PartNumber { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Qty { get; set; } = 0!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }

    }
    public class PartMappingProfile : Profile
    {
        public PartMappingProfile()
        {
            CreateMap<ApplicationPart, PartBriefDto>().ReverseMap();
        }
    }
}