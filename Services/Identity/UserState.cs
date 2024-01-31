using COM617.Data;

namespace COM617.Services.Identity
{
    /// <summary>
    /// Holds the current user's object.
    /// </summary>
    public class UserState
    {
        public User? CurrentUser { get; private set; }

        public event Action OnChange = null!;

        public void SetCurrentUser(User user)
        {
            CurrentUser = user;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
