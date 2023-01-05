using Microsoft.AspNetCore.Identity;

namespace Data.Models
{
    public class EmployeeClaim : IdentityUserClaim<string>
    {        
        public Employee? Employee { get; set; }
    }
}
