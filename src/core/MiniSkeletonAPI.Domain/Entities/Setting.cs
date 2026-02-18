using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Domain.Entities
{
    public class Setting : BaseEntity
    {
 
        public string? Name { get; set; }
        public string? Coler { get; set; }
        public int? StartRage{ get; set; }
        public int? EndRage { get; set; }
 
    }
}
