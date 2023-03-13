using System.Security.Claims;

namespace ToDoApi.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetEmail(this ClaimsPrincipal user)
        {
            var claims = user.Claims.ToList();
            var email = claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value;
            if (email == null || email == string.Empty)
            {
                throw new Exception("no email");
            }
            return email;
        }
    }
}