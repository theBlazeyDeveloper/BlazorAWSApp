using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Employee : IdentityUser
    {
        public Employee() : base() { }        

        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        [InverseProperty(nameof(Employee))]
        public ICollection<EmployeeRole>? EmployeeRoles { get; set; } = new List<EmployeeRole>();

        [InverseProperty(nameof(Employee))]
        public ICollection<EmployeeClaim>? EmployeeClaims { get; set; } = new List<EmployeeClaim>();

        [InverseProperty(nameof(Employee))]
        public ICollection<EmployeeLogin>? EmployeeLogins { get; set; } = new List<EmployeeLogin>();

        [InverseProperty(nameof(Employee))]
        public ICollection<EmployeeToken>? EmployeeTokens { get; set; } = new List<EmployeeToken>();

        public override string ToString()
        {
            return $"{LastName}, {FirstName}";
        }

        public void Deactivate()
        {
            IsActive = false;
            LockoutEnd = DateTimeOffset.MaxValue;
        }

        public void Activate()
        {
            IsActive = true;
            LockoutEnd = null;
        }
    }
}