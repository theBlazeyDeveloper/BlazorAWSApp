namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal p)
        {
            return p.Claims
                .FirstOrDefault(a => a.Type.Equals(ClaimTypes.NameIdentifier))?.Value ?? string.Empty;
        }
        public static string GetUsersName(this ClaimsPrincipal p)
        {
            return p.Claims
                .FirstOrDefault(a => a.Type.Equals(IdentityClaimTypes.UsersName))?.Value ?? string.Empty;
        }

        public static string FindFirst(this ClaimsPrincipal p, string type)
        {
            return p.Claims
                .FirstOrDefault(a => a.Type.Equals(type))?.Value ?? string.Empty;
        }

        public static bool IsAdmin(this ClaimsPrincipal p)
        {
            var c = p.Claims
                .FirstOrDefault(a => a.Value.Equals("admin"));

            if (c != null)
                return true;
            else
                return false;
        }
        public static bool IsReadonly(this ClaimsPrincipal p)
        {
            var c = p.Claims
                .FirstOrDefault(a => a.Value.Equals("admin"));

            if (c != null)
                return true;
            else
                return false;
        }
    }

    public static class IdentityClaimTypes
    {
        public const string FirstName = "FirstName";
        public const string LastName = "LastName";
        public const string IsActive = "IsActive";
        public const string UsersName = "UsersName";
    }
}
