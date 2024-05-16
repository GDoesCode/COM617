using COM617.Components.Nav;
using COM617.Services;
using COM617.Services.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace COM617.Shared
{
    public partial class MainLayout
    {
        private MobileNavbar? mobileNavbarRef;

        [Inject]
        private NavigationManager? navigationManager { get; set; }

        [Inject]
        private UserService? userService { get; set; }

        [Inject]
        private UserState? userState { get; set; }

        [Inject]
        private AuthenticationStateProvider? authenticationStateProvider { get; set; } = null;

        private async void CheckUser()
        {
            var state = await authenticationStateProvider!.GetAuthenticationStateAsync();
            var user = state.User;
            if (userState!.CurrentUser is null)
            {
                var mongoUser = userService!.GetUser(user.FindFirst("preferred_username")!.Value);
                if (mongoUser is null)
                    navigationManager!.NavigateTo("/register");
                else
                {
                    if (mongoUser.Id != userState!.CurrentUser?.Id)
                        userState.SetCurrentUser(mongoUser);
                }
            }
        }

        protected override void OnInitialized()
        {
            CheckUser();
            ModalService.ModalUpdated += (_, _) => InvokeAsync(StateHasChanged);
        }
    }
}
