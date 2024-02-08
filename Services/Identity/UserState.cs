using COM617.Data;

namespace COM617.Services.Identity
{
    /// <summary>
    /// Holds the current user's object.
    /// </summary>
    public class UserState
    {
        public User? CurrentUser { get; private set; }

        // OnChange asynchronous event
        public event EventHandler? OnChange;

        private readonly UserService? userService = null!;

        public UserState(UserService userService) => this.userService = userService;

        public void SetCurrentUser(User user)
        {
            CurrentUser = user;
            OnChange?.Invoke(this, EventArgs.Empty);
        }

        public async Task<bool> Save() => await userService!.UpdateUser(CurrentUser!);
    }
}
