using COM617.Data;
using COM617.Data.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace COM617.Services.Identity
{
    /// <summary>
    /// Custom manager used to provide a mechanism for signing users in and out.
    /// </summary>
    public class SignInService
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserService userService;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IUserClaimStore<User> userClaimStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignInService"/> class.
        /// </summary>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="userService">The user manager.</param>
        /// <param name="passwordHasher">The password hasher.</param>
        public SignInService(SignInManager<User> signInManager,
            UserService userService,
            IPasswordHasher<User> passwordHasher,
            IUserClaimStore<User> userClaimStore)
        {
            this.signInManager = signInManager;
            this.userService = userService;
            this.passwordHasher = passwordHasher;
            this.userClaimStore = userClaimStore;
        }

        /// <summary>
        /// Attempts to sign a user in, using the provided email and password.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        public async Task<SignInResult> PasswordSignInAsync(string email, string password)
        {
            var user = await userService.FindByEmailAsync(email);

            if (user == null)
                return SignInResult.Failed;

            var storedPassword = await userService.GetPasswordHashAsync(user, CancellationToken.None);
            var result = passwordHasher.VerifyHashedPassword(user, storedPassword!, password);

            if (result <= 0)
                return SignInResult.Failed;

            await signInManager.SignInWithClaimsAsync(user, true, user.Claims());
            await userClaimStore.AddClaimsAsync(user, user.Claims(), CancellationToken.None);
            return SignInResult.Success;
        }

        /// <summary>
        /// Attempts to sign a user in, using the provided user.
        /// </summary>
        /// <param name="user">The user.</param>
        public async Task<SignInResult> SignInAsync(User user)
        {
            await signInManager.SignInWithClaimsAsync(user, true, user.Claims());
            await userClaimStore.AddClaimsAsync(user, user.Claims(), CancellationToken.None);
            return SignInResult.Success;
        }

        /// <summary>
        /// Removes claims for the current user, and signs them out.
        /// </summary>
        public async Task SignOutAsync(ClaimsPrincipal principal)
        {
            //await userClaimStore.RemoveClaimsAsync(userService.CurrentUser(), principal.Claims, CancellationToken.None);
            await signInManager!.SignOutAsync();
        }
    }
}
