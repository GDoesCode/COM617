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
    public class User
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = string.Empty;
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public UserRole Role { get; set; }

        public User(string email, UserRole role = UserRole.Standard, string firstName = "", string lastName = "")
        {
            Role = role;
            Email = email;
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