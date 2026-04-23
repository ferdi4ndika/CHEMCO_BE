using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Domain.Entities
{
    public class Op : BaseEntity
    {
        public string? Name { get;set; }
        //[Column("code_1")]
        public string? Code { get;set; }
    
    }
}

