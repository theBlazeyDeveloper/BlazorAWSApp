using Microsoft.AspNetCore.Identity;

namespace Shared.Results
{
    /// <summary>
    /// Class used to display messages to the UI regarding operation results
    /// </summary>
    public readonly struct Result
    {
        public Result(IdentityResult result, string messagePreFix)
        {
            Succeeded = result.Succeeded;

            if (result.Succeeded)
                Message = $"{messagePreFix} was successful";
            else
                Message = $"{messagePreFix} Failed: {string.Join(", ", result.Errors.Select(a => a.Description))}";
        }
        public Result(bool succeeded, string messagePreFix, string[] errors = default!)
        {
            Succeeded = succeeded;

            if (succeeded)
                Message = $"{messagePreFix} was successful";
            else
                Message = $"{messagePreFix} Failed: {string.Join(", ", errors)}";
        }

        /// <summary>
        /// Message to be displayed
        /// </summary>
        public string Message { get; }
        /// <summary>
        /// Flag indicating whether the operation was successful
        /// </summary>
        public bool Succeeded { get; }

        /// <summary>
        /// The color of the messages text when rendered in the UI
        /// </summary>
        public string TextColor { get => Succeeded ? "text-success" : "text-danger"; }

        public override string ToString()
        {
            return $"{Succeeded.ToSuccessFailure()}: {Message}";
        }
    }
}
