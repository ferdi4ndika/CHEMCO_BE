using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Infrastructure.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole(string Name)
       : base(Name) { }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        public DateTime? LastCreated { get; set; }

        [NotMapped]
        public List<ApplicationPermissionClaim> Claims { get; set; }
    }
}
