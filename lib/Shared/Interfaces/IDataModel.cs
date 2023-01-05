using System.Security.Claims;

namespace Shared.Interfaces
{
    public interface IDataModel
    {
        /// <summary>
        /// Primary Key of the entity
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Flag for soft deletes
        /// </summary>
        bool IsDeleted { get; }

        /// <summary>
        /// Timestamp for when entity is created
        /// </summary>
        DateTime Created { get; }

        /// <summary>
        /// Timestamp of when the entity is updated
        /// </summary>
        DateTime Modified { get; }

        /// <summary>
        /// Id of the user who performed the last operation on this entity
        /// </summary>
        string? EmployeeId { get; }

        /// <summary>
        /// Current authenticated user
        /// </summary>
        ClaimsPrincipal? User { get; }

        /// <summary>
        /// Method used when new entity is created in the database
        /// </summary>
        void OnCreated();
        /// <summary>
        /// Method used when new entity is updated in the database
        /// </summary>
        void OnUpdated();

        /// <summary>
        /// Method used when new entity is deleted in the database
        /// </summary>
        void OnDelete();
    }
}
