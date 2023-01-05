namespace Shared.Results
{
    /// <summary>
    /// Class used to display multiple messages to the UI regarding operation results
    /// </summary>
    public readonly struct DisplayableResults
    {
        readonly IList<Result> _results = new List<Result>();

        public DisplayableResults() { }

        public DisplayableResults(IList<Result> results)
        {
            _results = results;
        }

        /// <summary>
        /// The list of results to display
        /// </summary>
        public IList<Result> Results { get => _results; }

        /// <summary>
        /// Adds a result object to the list of results
        /// </summary>
        /// <param name="r"></param>
        public void AddResult(Result r) => _results.Add(r);

        /// <summary>
        /// Clears the list of results
        /// </summary>
        public void Clear() => _results.Clear();

        public override string ToString()
        {            
            return string.Join(", ", _results.Select(a => a.Message));
        }
    }
}
