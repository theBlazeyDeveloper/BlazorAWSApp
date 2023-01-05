namespace System
{
    public static class BoolExtensions
    {
        /// <summary>
        /// Returns yes if true and no if false
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToYesNo(this bool b) => b ? "Yes" : "No";

        /// <summary>
        /// Returns Successful if true and Failed if false
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToSuccessFailure(this bool b) => b ? "Succeeded" : "Failed";

        /// <summary>
        /// Returns Active if true and In-Active if false
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToActiveInActive(this bool b) => b ? "Active" : "In-Active";

        /// <summary>
        /// Evaluates a given string and tries to convert it to a bool value
        /// </summary>
        /// <param name="s">string value</param>
        /// <param name="result">Whether the operation was successful</param>
        /// <returns></returns>
        public static bool TryParseBoolean(this string s, out bool result)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    result = false;
                    return true;
                }
                else
                {
                    if (s == "true" || s == "false")
                        result = Convert.ToBoolean(s);
                    else
                    {
                        var intValue = Convert.ToInt32(s);
                        result = Convert.ToBoolean(intValue);
                    }
                    return true;
                }
            }
            catch
            {                
                result = false;
                return false;
            }
        }
    }
}
