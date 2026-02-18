using System;

namespace MiniSkeletonAPI.Domain.Entities
{
    public class DataAndon : BaseEntity
    {
        public string? PartName { get; set; }
        public string? Coler { get; set; }
        public string? PartNumber { get; set; }
        public Guid? IdType { get; set; }
        public string? Description { get; set; }
        public string? LotMaterial { get; set; }
        public string? Antrian { get; set; }
        public string? Repair { get; set; }
        public int? Status { get; set; }
        public float? HangerSpeed { get; set; }
        public DateTime? StarProsess { get; set; }
        public float? stopLine { get; set; }
        public int? QtyPart { get; set; }
        public int? QtyHangar { get; set; }
   
    }
}
