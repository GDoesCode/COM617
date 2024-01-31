using COM617.Services;

using Microsoft.AspNetCore.Identity;
using System.Security.Principal;

namespace COM617.Data
{
    /// <summary>
    /// Primary user class for AFST.
    /// </summary>
    /// <seealso cref="IdentityUser" />
    [MongoTypeMap("com617", "Users")]
    public class User : IdentityUser
    {
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Standard;

        public User(string firstname, string lastname, string email, UserRole role = UserRole.Standard)
        {
            Role = role;
            UserName = email;
            Email = email;
            Firstname = firstname;
            Lastname = lastname;
        }
        public User() { }

        public bool ChangeRole(User currentUser, UserRole role)
        {
            // Guard clause preventing non-admin users from making the change.
            if (currentUser.Role >= UserRole.Standard)
            {
                return false;
            }

            Role = role;
            return true;
        }

        public override string ToString() => $"{Firstname} {Lastname}";
    }

    /// <summary>
    /// Enum holding values to differentiate user roles.
    /// </summary>
    public enum UserRole
    {
        Admin,
        Standard
    }
}