using COM617.Services;
using Dstl.Afst.Core.Data.Identity;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver.Linq;
using System.Security.Claims;

namespace COM617.Data.Identity
{
    /// <summary>
    /// Primary user store for AFST users.
    /// Used for interacting with mongo to CRUD users.
    /// </summary>
    /// <seealso cref="IUserStore{User}" />
    /// <seealso cref="IUserEmailStore{User}" />
    /// <seealso cref="IUserPasswordStore{User}" />
    /// <seealso cref="IUserClaimStore{User}" />
    public class UserStore : IUserStore<User>, IUserEmailStore<User>, IUserPasswordStore<User>, IUserClaimStore<User>
    {
        private readonly IMongoQueryable<User> identityCollection;
        private readonly IMongoQueryable<UserClaim> claimsCollection;
        private readonly MongoDbService mongoDbService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserStore"/> class.
        /// </summary>
        /// <param name="mongoDbService">The mongo database service.</param>
        public UserStore(MongoDbService mongoDbService)
        {
            this.mongoDbService = mongoDbService;
            identityCollection = mongoDbService.GetQueryableCollection<User>();
            claimsCollection = mongoDbService.GetQueryableCollection<UserClaim>();
        }

        /// <summary>
        /// Gets a value indicating whether this is the first run of the system.
        /// </summary>
        public IMongoQueryable<User> CurrentUsers => identityCollection;

        /// <inheritdoc />
        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            var existingUser = await FindByEmailAsync(user.Email!, cancellationToken);

            if (existingUser != null)
            {
                var error = CustomIdentityError.USER_ALREADY_EXISTS;

                return IdentityResult.Failed(error);
            }

            await mongoDbService.CreateDocument(user);

            return IdentityResult.Success;
        }

        /// <inheritdoc />
        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(); // TODO: Support deleting a user from the db and returning the result.
        }

        /// <inheritdoc />
        public void Dispose() { }

        /// <inheritdoc />
        public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await Task.FromResult(identityCollection.FirstOrDefault(x => x.Email == email));
        }

        /// <inheritdoc />
        public async Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await Task.FromResult(identityCollection.FirstOrDefault(x => x.Id == userId));
        }


        /// <inheritdoc />
        public async Task<string?> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Email);
        }

        /// <inheritdoc />
        public async Task<string?> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            if (identityCollection.FirstOrDefault(x => x.Id == user.Id) != null)
            {
                return await Task.FromResult(identityCollection.FirstOrDefault(x => x.Id == user.Id)!.PasswordHash);
            }
            return null!;
        }

        /// <inheritdoc />
        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            if (identityCollection.FirstOrDefault(x => x.Id == user.Id) != null)
            {
                return await Task.FromResult(identityCollection.FirstOrDefault(x => x.Id == user.Id)!.Id.ToString());
            }
            return null!;
        }

        /// <inheritdoc />
        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
            => await Task.FromResult(user.PasswordHash != null);

        /// <inheritdoc />
        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            try
            {
                var existingClaim = claimsCollection.AsEnumerable().First(x => x.UserId == user.Id);

                if (existingClaim != null)
                {
                    await mongoDbService.ReplaceDocument(existingClaim.Id, existingClaim.Duplicate(claims));
                }
            }
            catch (InvalidOperationException)
            {
                await mongoDbService.CreateDocument(new UserClaim(user.Id, claims));
            }
        }

        /// <inheritdoc />
        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken)
        {
            var existingClaims = claimsCollection.FirstOrDefault(x => x.UserId == user.Id);

            return existingClaims != null ? existingClaims.Claims : await Task.FromResult<IList<Claim>>(new List<Claim>());
        }

        /// <inheritdoc />
        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            var usersWithDesiredClaim = claimsCollection 
                .AsEnumerable() // Apparently, performing ".Where" on a mongoQueryable doesn't work, so we cast it to an enumerable first
                .Where(userClaim => userClaim.Claims
                .Any(c => c == claim))
                .Select(userClaim => userClaim.UserId).ToList();

            return await Task.FromResult(identityCollection.AsEnumerable().Where(user => usersWithDesiredClaim.Contains(user.Id)).ToList());
        }

        /// <inheritdoc />
        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            // Just remove all existing claims for existingClaims user.
            // In the future, we will probably want to remove only the claims received in this method
            var storedClaim = claimsCollection.AsEnumerable().Where(x => x.UserId == user.Id);
            if (storedClaim.Any())
            {
                await mongoDbService.DeleteDocument<UserClaim>(storedClaim.First().Id);
            }
        }

        /// <inheritdoc />
        public async Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            // Find the claim record for the user
            var userClaim = claimsCollection.Where(x => x.UserId == user.Id).First();
            // Get the index of the claim to be replaced
            var index = userClaim.Claims.ToList().IndexOf(claim);
            // Replace the claim at existingClaims position
            userClaim.Claims[index] = newClaim;

            // Replace the document
            await mongoDbService.ReplaceDocument(userClaim.Id, userClaim);
        }

        /// <inheritdoc />
        public async Task SetEmailAsync(User user, string? email, CancellationToken cancellationToken)
            => await Task.Run(() => user.Email = email);

        /// <inheritdoc />
        public async Task SetPasswordHashAsync(User user, string? passwordHash, CancellationToken cancellationToken)
            => await Task.Run(() => user.PasswordHash = passwordHash);


        /// <inheritdoc />
        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(); // When do we need this?
        }

        public Task<string?> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email)!;
        }

        public Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedEmailAsync(User user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
