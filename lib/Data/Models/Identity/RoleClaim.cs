using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class RoleClaim : IdentityRoleClaim<string>
    {
        [ForeignKey(nameof(RoleId))]
        public Role? Role { get; set; }
    }    
}
