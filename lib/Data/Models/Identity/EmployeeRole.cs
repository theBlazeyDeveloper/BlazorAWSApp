using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class EmployeeRole : IdentityUserRole<string>
    {
        [ForeignKey(nameof(UserId))]
        public Employee? Employee { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role? Role { get; set; }
    }
}
