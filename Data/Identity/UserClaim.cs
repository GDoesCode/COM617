using COM617.Services;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace COM617.Data.Identity
{
    /// <summary>
    /// Holds information about a <see cref="User"/>s claims.
    /// </summary>
    [MongoTypeMap("COM617_Identity", "Claims")]
    internal class UserClaim
    {
        /// <summary>
        /// Gets the claim identifier.
        /// </summary>
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public string UserId { get; private set; }

        /// <summary>
        /// Gets the claims.
        /// </summary>
        public Claim[] Claims { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserClaim"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="claims">The claims.</param>
        public UserClaim(string userId, IEnumerable<Claim> claims)
        {
            UserId = userId;
            Claims = claims.ToArray();
        }

        /// <summary>
        /// Returns a copy of an existing <see cref="UserClaim"/>, with the same Id, but with updated claims.
        /// </summary>
        /// <param name="claims">The claims.</param>
        public UserClaim Duplicate(IEnumerable<Claim> claims) => new(UserId, claims)
        {
            Id = Id
        };
    }
}
