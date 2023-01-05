using Shared.Results;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Shared.Abstract
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        bool _isLoading;

        public ViewModelBase(ClaimsPrincipal user)
        {
            Id = Guid
                .NewGuid()
                .ToString();

            IsDeleted = false;
            Created = DateTime.Now;
            Modified = DateTime.Now;
            EmployeeId = user.GetUserId();
            IsNew = true;

            User = user;
        }        
        protected ViewModelBase(DataModelBase e, ClaimsPrincipal user)
        {
            Id = e.Id;
            IsDeleted = e.IsDeleted;
            Created = e.Created;
            Modified = e.Modified;
            EmployeeId = user.GetUserId();
            IsNew = false;

            User = user;
        }

        /// <summary>
        /// Entity Property
        /// </summary>
        [Display(Name = "Id")]
        public virtual string Id { get; set; } = default!;
        /// <summary>
        /// Entity Property
        /// </summary>
        [Display(Name = "Deleted?")]
        public virtual bool IsDeleted { get; set; }
        /// <summary>
        /// Entity Property
        /// </summary>
        [Display(Name = "Created")]
        public virtual DateTime Created { get; set; }
        /// <summary>
        /// Entity Property
        /// </summary>
        [Display(Name = "Modified")]
        public virtual DateTime Modified { get; set; }
        /// <summary>
        /// Entity Property
        /// </summary>
        [Display(Name = "Employee")]
        public virtual string EmployeeId { get; set; } = default!;
        /// <summary>
        /// The current authenticated user
        /// </summary>
        public virtual ClaimsPrincipal User { get; }
        /// <summary>
        /// UI Property
        /// </summary>
        public Result? Result { get; set; }
        /// <summary>
        /// UI Property
        /// </summary>
        public DisplayableResults Results { get; set; } = new();

        /// <summary>
        /// UI Property
        /// </summary>
        public string ResultTextColor => Result.HasValue && Result.Value.Succeeded ? "text-success" : "text-danger";

        /// <summary>
        /// UI Property
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// UI Property
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            private set
            {
                _isLoading = value;

                OnPropertyChanged();
            }
        }
        /// <summary>
        /// UI Property
        /// </summary>
        public virtual string ListPageTitle { get; set; } = default!;
        /// <summary>
        /// UI Property
        /// </summary>
        public virtual string AddUrl { get; set; } = default!;
        /// <summary>
        /// UI Property
        /// </summary>
        public virtual string BackUrl { get; set; } = default!;
        /// <summary>
        /// UI Method that can be used to display a loading element
        /// </summary>
        public void Loading() => _isLoading = true;
        /// <summary>
        /// UI Method that can be used to hide a loading element
        /// </summary>
        public void NotLoading() => _isLoading = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = default!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Method that returns the entity state to a "Newish" state while preserving the rest of the model
        /// </summary>
        public virtual void Clear()
        {
            Id = Guid
                .NewGuid()
                .ToString();

            IsDeleted = false;
            Created = DateTime.Now;
            Modified = DateTime.Now;

            IsNew = true;
        }

        /// <summary>
        /// Method that sets the result object
        /// </summary>
        public virtual void SetResult(Result? result)
        {
            if (result.HasValue)
            {
                Result = result;

                OnPropertyChanged();                
            }
        }

        /// <summary>
        /// Adds a result object to the list of results in DisplayableResult object
        /// </summary>
        /// <param name="result"></param>
        public virtual void AddResult(Result? result)
        {
            if (result.HasValue)
            {
                Results.AddResult(result.Value);

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Method that sets the result object
        /// </summary>
        public virtual void SetResults(DisplayableResults? results)
        {
            if (results.HasValue)
            {
                Results = results.Value;

                OnPropertyChanged();                
            }
        }

        /// <summary>
        /// Resets Result state to null
        /// </summary>
        public virtual void ClearResult()
        {
            Result = null;
            OnPropertyChanged();
        }
        /// <summary>
        /// Resets Results state to null
        /// </summary>
        public virtual void ClearResults()
        {
            Results = new();
            OnPropertyChanged();
        }
    }

    public abstract class ViewModelBase<T> : ViewModelBase where T : class
    {
        HashSet<T> _models = new();

        protected ViewModelBase(ClaimsPrincipal user) : base(user) { }         
        
        public ViewModelBase(DataModelBase e, ClaimsPrincipal user) : base(e, user){ }     

        /// <summary>
        /// Hash set of View models of type T
        /// </summary>
        public virtual HashSet<T> Models 
        {
            get => _models;
            set => _models = value;
        }

        /// <summary>
        /// Clears the list of ViewModels
        /// </summary>
        public virtual void ClearModels()
        {
            _models.Clear();

            OnPropertyChanged();
        }
        /// <summary>
        /// Adds an Enumerable list of ViewModels to the Models property
        /// </summary>
        /// <param name="models">List of ViewModels</param>
        public virtual void SetModels(IEnumerable<T> models)
        {
            _models = models.ToHashSet();

            OnPropertyChanged();
        }
        /// <summary>
        /// Adds a single ViewModel to the list of Models
        /// </summary>
        /// <param name="model"></param>
        public virtual void AddModel(T model)
        {
            _models.Add(model);            

            OnPropertyChanged();
        }                
    }
}
