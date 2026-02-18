using System;

namespace MiniSkeletonAPI.Domain.Entities
{
    public class DataCounting : BaseEntity
    {
        public int? CountNumber { get; set; }
        public float? Speed { get; set; }

        public MWarna? data {  get; set; }

    }
}
