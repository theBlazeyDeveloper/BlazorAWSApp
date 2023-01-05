using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class EmployeeLogin : IdentityUserLogin<string>
    {
        [ForeignKey(nameof(UserId))]
        public Employee? Employee { get; set; }
    }
}
