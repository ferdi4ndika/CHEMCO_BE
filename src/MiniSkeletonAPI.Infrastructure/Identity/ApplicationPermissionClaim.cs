using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Infrastructure.Identity
{
    public class ApplicationPermissionClaim : IdentityRoleClaim<string>
    {
        public DateTime Last_Created { get; set; }
    }
}
