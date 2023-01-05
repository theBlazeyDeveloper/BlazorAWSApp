using Shared.Abstract;
using Shared.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared
{
    /// <summary>
    /// Base model for all entities saved to the database
    /// </summary>
    public abstract class DataModelBase : IDataModel
    {
        protected DataModelBase()
        {
            Id = Guid
                .NewGuid()
                .ToString();

            Created = DateTime.Now;
            Modified = DateTime.Now;
            IsDeleted = false;
        }
        protected DataModelBase(DTOModelBase e)
        {
            Id = e.Id;
            Created = e.Created;
            Modified = e.Modified;
            IsDeleted = e.IsDeleted;
            EmployeeId = e.EmployeeId;
        }
        protected DataModelBase(ViewModelBase m)
        {
            Id = m.Id;
            IsDeleted = m.IsDeleted;
            Created = m.Created;
            Modified = m.Modified;
            EmployeeId = m.EmployeeId;

            User = m.User;
        }
        
        [Key]
        public string Id { get; init; }        
        public bool IsDeleted { get; set; }        
        public DateTime Created { get; set; }        
        public DateTime Modified { get; set; }
        public string EmployeeId { get; set; } = string.Empty;

        [NotMapped]
        [JsonIgnore]
        public ClaimsPrincipal? User { get; }

        public virtual void OnCreated()
        {
            Created = DateTime.Now;
            Modified = DateTime.Now;
            IsDeleted = false;

            if (User is not null)
                EmployeeId = User.GetUserId();
        }
        public virtual void OnUpdated()
        {
            Modified = DateTime.Now;

            if (User is not null)
                EmployeeId = User.GetUserId();
        }
        public virtual void OnDelete()
        {
            IsDeleted = true;
            Modified = DateTime.Now;

            if (User is not null)
                EmployeeId = User.GetUserId();
        }
        public virtual void SetEmployeeId(string employeeId)
            => EmployeeId = employeeId;
        public virtual void SetModifiedDate(DateTime date)
            => Modified = date;

        /// <summary>
        /// Allow each entity to handle how it serializes it's self in order to flatten data thats recorded
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        public abstract string Serialize(JsonSerializerOptions opts = default!);

        public override bool Equals(object? obj) 
            => obj is DataModelBase other && Id.Equals(other.Id);        

        public override int GetHashCode() 
            => Id.GetHashCode();

        public abstract bool Search(string value);       
    }

    /// <summary>
    /// Generic Base model for all entities saved to the database
    /// </summary>
    public abstract class DataModelBase<T> : DataModelBase where T : class, new()
    {
        protected DataModelBase() : base() { }

        protected DataModelBase(DTOModelBase m) : base(m) { }

        protected DataModelBase(ViewModelBase m) : base(m) { }        
    }
}
