using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Domain.Entities
{
    public class Op : BaseEntity
    {
        public string? Name { get;set; }
        public string? Code { get;set; }
    }
}

