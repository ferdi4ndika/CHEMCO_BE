using AutoMapper;
using MiniSkeletonAPI.Application.Identity.Images.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSkeletonAPI.Infrastructure.Identity
{
    public class ApplicationImage
    {
        public ApplicationImage(string name, string code )
        {
            Name = name;
            Code = code;
    
            CreatedAt = DateTime.UtcNow;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = null!;
        public string Code  { get; set; } = null!; 
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; } 

        /*[NotMapped]
        public List<ApplicationPermissionClaim> Claims { get; set; } = new List<ApplicationPermissionClaim>();*/
    }
    public class ImageMappingProfile : Profile
    {
        public ImageMappingProfile()
        {
            CreateMap<ApplicationImage, ImageBriefDto>().ReverseMap();
        }
    }
}
