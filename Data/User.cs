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
        public string Title { get; set; }
        public UserRole Role { get; set; }
        public string ProfilePicUrl { get; set; }

        private readonly List<string> defaultImages = new();

        public User(string email, UserRole role = UserRole.Standard, string firstName = "", string lastName = "", string title = "")
        {
            Role = role;
            Email = email;
            Firstname = firstName;
            Lastname = lastName;
            Title = title;
            Random rnd = new();
            defaultImages.Add("https://images.unsplash.com/photo-1517841905240-472988babdf9?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80");
            defaultImages.Add("https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80");
            ProfilePicUrl = defaultImages[rnd.Next(1)];
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