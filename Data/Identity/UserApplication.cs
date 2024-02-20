using COM617.Services;
using MongoDB.Bson.Serialization.Attributes;

namespace COM617.Data
{
    [MongoTypeMap("com617", "UserApplications")]
    public class UserApplication
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();
        public User User { get; set; }
        public DateOnly DateCreated { get; set; }

        public UserApplication(User user, DateOnly? dateCreated = null)
        {
            User = user;
            _ = dateCreated == null ? DateCreated = DateOnly.FromDateTime(DateTime.UtcNow) : DateCreated = dateCreated.Value;
        }
    }
}
