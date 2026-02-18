using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Domain.Entities
{
    public record User 
    {
        public string? Password { get; init; }
        public string UserName { get; init; }
        public string Nik { get; init; }
        public string Role { get; init; }

    }
}
