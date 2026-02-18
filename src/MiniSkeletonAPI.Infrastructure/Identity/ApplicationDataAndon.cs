using AutoMapper;
using MiniSkeletonAPI.Application.Identity.DataAndons.Dtos;
using Serilog;
using System;

namespace MiniSkeletonAPI.Infrastructure.Identity
{
    public class ApplicationDataAndon
    {
        public ApplicationDataAndon() { }

        public ApplicationDataAndon(string name, string description,string lotMaterial, string coler, string type, string idType, int qtyPart, int status, int qtyHangar, string repair)
        {
            Id = Guid.NewGuid().ToString();
            PartName = name;
            Description = description;
            LotMaterial = lotMaterial;
            Repair = repair;
            Coler = coler;
            PartNumber = type;
            IdType = idType;
            QtyPart = qtyPart;
            Status = status;
            QtyHangar = qtyHangar;
            CreatedAt = DateTime.UtcNow;
            UpdateAt = DateTime.UtcNow;
        }

        public string Id { get; set; }
        public string PartName { get; set; } = null!;
        public string? Coler { get; set; }
        public string? PartNumber { get; set; }
        public string? IdType { get; set; }
        public string? Description { get; set; }
        public string? LotMaterial { get; set; }
        public string? Repair { get; set; }
        public int? Status { get; set; }
        public int QtyPart { get; set; }
        public int QtyHangar { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }

    public class DataAndonMappingProfile : Profile
    {
        public DataAndonMappingProfile()
        {
            CreateMap<ApplicationDataAndon, DataAndonBriefDto>().ReverseMap();
        }
    }
}
