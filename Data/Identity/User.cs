using COM617.Services;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization.Attributes;

namespace COM617.Data
{
    /// <summary>
    /// User data class.
    /// </summary>
    /// <seealso cref="IdentityUser" />
    [MongoTypeMap("com617", "Users")]
    public class User : IdentityUser
    {
        public UserRole Role { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public User(string email, string firstName, string lastName, UserRole role = UserRole.Standard)
        {
            Role = role;
            Email = email;
            Id = Email;
            Firstname = firstName;
            Lastname = lastName;
        }

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