using System.Security.Claims;

namespace COM617.Data.Identity
{
    /// <summary>
    /// Extension methods for <see cref="User"/>.
    /// </summary>
    public static class IdentityExtensions
    {
        /// <summary>
        /// Gets the claims for a user.
        /// </summary>
        /// <remarks>
        /// Because <see cref="UserRole.Admin"/> is the highest level of privilege, it is position 0 in the enum.
        /// E.g. Deveoper = 0, Core = 2, etc. 
        /// Therefore, we cascade down the list, which actually cascades up in the integer value. If you are a admin,
        /// you will receive a claim for each role that you belong to (for a developer will be every role).
        /// If you are a <see cref="UserRole.Standard"/>, you will receive a claim for standard, and for admin.
        /// This is to prevent claim logic from flooding the UI, and will simply require the use of the AuthorizeView component
        /// to specify a single role, and everyone with claims for that role, will be authorised against it.
        /// Also returns First and Last name claims.
        /// </remarks>
        /// <param name="user">The user.</param>
        public static IEnumerable<Claim> Claims(this User user)
        {
            var claims = new List<Claim>();
            var userRole = (int)user.Role;
            var highestRole = (int)Enum.GetValues(typeof(UserRole)).Cast<UserRole>().Last();

            for (int i = userRole; i <= highestRole; i++)
            {
                claims.Add(new Claim(ClaimTypes.Role, Enum.GetName(typeof(UserRole), i)!));
            }

            claims.Add(new Claim(ClaimTypes.GivenName, user.Firstname));
            claims.Add(new Claim(ClaimTypes.Surname, user.Lastname));

            return claims;
        }

        /// <summary>
        /// Returns the concatenated full name of a given <see cref="ClaimsPrincipal"/>.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        public static string FullName(this ClaimsPrincipal claimsPrincipal)
            => $"{claimsPrincipal.Claims.First(x => x.Type == ClaimTypes.GivenName).Value} {claimsPrincipal.Claims.First(x => x.Type == ClaimTypes.Surname).Value}";
    }
}
