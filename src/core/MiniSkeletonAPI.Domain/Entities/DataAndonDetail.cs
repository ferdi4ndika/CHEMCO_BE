using System;

namespace MiniSkeletonAPI.Domain.Entities
{
    public class DataAndonDetail : BaseEntity
    {
        public string? PartName { get; set; }
        public string? Coler { get; set; }
        public string? PartNumber { get; set; }
        public Guid? IdType { get; set; }
        public Guid ? IdAndon { get; set; }
        public int? CountNumber { get; set; }
        public int? Qty { get; set; }
        public string? Description { get; set; }
        public string? LotMaterial { get; set; }
        public string? Repair { get; set; }
        public DateTime? Step1 { get; set; } = null;
        public DateTime? Step2 { get; set; } = null;
        public DateTime? Step3 { get; set; } = null;
        public DateTime? Step4 { get; set; } = null;
        public DateTime? Step5 { get; set; } = null;
        public DateTime? Step6 { get; set; } = null;
    }
}
