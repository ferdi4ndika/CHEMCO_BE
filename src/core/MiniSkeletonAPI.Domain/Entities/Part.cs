using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Domain.Entities
{
    public class Part : BaseEntity
    {
        public string? PartName { get; set; }
        public string? PartNumber { get; set; }
        public string? Description { get; set; }
        public int? Qty { get; set; }
    }
}
