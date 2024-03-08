using Amazon.Auth.AccessControlPolicy;
using COM617.Services.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Security.Principal;

namespace COM617.Pages
{
    public partial class Index
    {
        [Inject]
        private UserService UserService { get; set; } = null!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;


        protected override void OnAfterRender(bool firstRender)
        {
            var state = AuthenticationStateProvider.GetAuthenticationStateAsync().GetAwaiter().GetResult();
            var principal = state.User;
            var check = principal.Claims.Any(x => x.ValueType == ClaimTypes.Role);
            if (!check)
            {
                var mongoUser = UserService.GetUser(principal.Identity!.Name!).GetAwaiter().GetResult();
                if (mongoUser != null)
                {
                    NavigationManager.NavigateTo("/SignInMs", true);
                }
                else
                {
                    NavigationManager.NavigateTo("/Register");
                }
            }
        }
        public void SignIn(ClaimsPrincipal principal)
        {
            
        }
    }
}
