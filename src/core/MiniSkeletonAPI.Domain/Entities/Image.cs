using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Domain.Entities
{
    public record Image
    {
        public string? Name { get; init; }
        public string? Code { get; init; }
    }
}
