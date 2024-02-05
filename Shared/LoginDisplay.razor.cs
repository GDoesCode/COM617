using COM617.Data;
using COM617.Services.Identity;
using Microsoft.AspNetCore.Components;

namespace COM617.Shared
{
    public partial class LoginDisplay
    {
        private bool collapseProfileMenu { get; set; } = true;

        [Inject]
        private UserState UserState { get; set; } = null!;

        private User? User => UserState.CurrentUser;

        protected override void OnInitialized()
        {
            UserState.OnChange += async (_, _) => await InvokeAsync(StateHasChanged);
        }
    }
}
