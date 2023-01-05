using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Role : IdentityRole<string>
    {
        public Role() : base()
        { }

        public Role(string roleName) : base(roleName)
        {
            Id = Guid.NewGuid().ToString();
        }

        [InverseProperty(nameof(Role))]
        public ICollection<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();

        [InverseProperty(nameof(Role))]
        public ICollection<EmployeeRole> EmployeeRoles { get; set; } = new List<EmployeeRole>();        
    }
}
