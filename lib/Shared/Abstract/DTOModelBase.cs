using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.Abstract
{
    public abstract class DTOModelBase
    {
        protected DTOModelBase(DataModelBase e)
        {
            Id = e.Id;
            Created = e.Created;
            Modified = e.Modified;
            IsDeleted = e.IsDeleted;
            EmployeeId = e.EmployeeId;
        }

        protected DTOModelBase()
        { }

        [Display(Name = "Id")]
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [Display(Name = "Is Deleted?")]
        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Created")]
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [Display(Name = "Modified")]
        public DateTime Modified { get; set; }

        [Display(Name = "Employee")]
        [JsonPropertyName("employeeId")]
        public string EmployeeId { get; set; } = string.Empty;
    }
}
