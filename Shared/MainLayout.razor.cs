using COM617.Components.Nav;
using COM617.Data.Identity;
using COM617.Services.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace COM617.Shared
{
    public partial class MainLayout
    {
        private MobileNavbar? mobileNavbarRef;

        [Inject]
        private UserService userService { get; set; } = null!;

        [Inject]
        private SignInService signInService { get; set; } = null!;

        [Inject]
        private AuthenticationStateProvider? authenticationStateProvider { get; set; } = null!;

        [Inject]
        private NavigationManager? navigationManager { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            var state = await authenticationStateProvider!.GetAuthenticationStateAsync();
            var user = state.User;
            if (!user.Claims.Any(x => x.Type == ClaimTypes.Role)) 
            {
                var mongoUser = await userService.GetUser(user.Identity!.Name!);
                if (mongoUser is not null)
                {
                    navigationManager!.NavigateTo("/AddClaims");
                    await signInService.SignInAsync(mongoUser);
                } else
                {
                    navigationManager!.NavigateTo("/Register");
                }
            }
        }
    }
}
