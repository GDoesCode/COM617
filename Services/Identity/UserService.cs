using COM617.Data;
using COM617.Data.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using System.Security.Claims;

namespace COM617.Services.Identity
{
    /// <summary>
    /// Handles interactions with the User and UserApplication collections in MongoDb.
    /// </summary>
    public class UserService
    {
        private readonly MongoDbService mongoDbService;
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly UserStore? userStore;
        private readonly IUserEmailStore<User> userEmailStore;
        private readonly IUserPasswordStore<User> userPasswordStore;

        public UserService(IUserStore<User> userStore, MongoDbService mongoDbService, AuthenticationStateProvider authenticationStateProvider)
        {
            this.userStore = (UserStore?)userStore;
            userEmailStore = (IUserEmailStore<User>)userStore;
            userPasswordStore = (IUserPasswordStore<User>)userStore;
            this.authenticationStateProvider = authenticationStateProvider;
            this.mongoDbService = mongoDbService;
        }

        public async Task<User?> GetUser(string email) => await userEmailStore.FindByEmailAsync(email, CancellationToken.None);

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await userEmailStore.FindByEmailAsync(email, CancellationToken.None);
        }

        public async Task<string?> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return await userPasswordStore.GetPasswordHashAsync(user, CancellationToken.None);
        }

        public async Task<bool> CreateUser(User user)
        {
            var dbUser = await GetUser(user.Email!);
            if (dbUser is null)
            {
                await mongoDbService.CreateDocument(user);
                return true;
            } 
            else
                return false;
        }

        public async Task<bool> UpdateUser(User user)
        {
            var dbUser = GetUser(user.Email!);
            if (dbUser is null)
                return false;
            else
            {
                await mongoDbService.ReplaceDocument(user.Id, user);
                return true;
            }
        }

        public UserApplication? GetUserApplication(string email) => mongoDbService.GetDocumentsByFilter<UserApplication>(app => app.User.Email == email).FirstOrDefault();

        public async Task CreateUserApplication(UserApplication userApplication) => await mongoDbService.CreateDocument(userApplication);

        /// <inheritdoc />
        public User CurrentUser() => userStore!.CurrentUsers.First(x => x.Email == CurrentUserEmail);

        /// <inheritdoc />
        public string CurrentUserEmail => GetCurrentUserEmail().GetAwaiter().GetResult();

        /// <inheritdoc />
        public string FullName(Guid? id)
        {
            if (id == null)
                return string.Empty;

            var user = GetUserById(id.Value);
            return user.ToString();
        }

        /// <inheritdoc />
        public string FullName(ClaimsPrincipal principal)
        {
            var firstname = principal.Claims.First(x => x.Type == ClaimTypes.GivenName).Value;
            var secondname = principal.Claims.First(x => x.Type == ClaimTypes.Surname).Value;

            return $"{firstname} {secondname}";
        }

        private User GetUserById(Guid id) => userStore!.CurrentUsers.First(x => x.Id == id);

        private async Task<string> GetCurrentUserEmail()
        {
            var state = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = state.User;

            return user.Claims.First(x => x.Type == "preferred_username").Value;
        }
    }
}
