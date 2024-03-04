using COM617.Components.Nav;
using COM617.Data;
using COM617.Services;
using COM617.Services.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Principal;

namespace COM617.Shared
{
    public partial class MainLayout
    {
        private MobileNavbar? mobileNavbarRef;
        private IIdentity? identity;

        [Inject]
        private NavigationManager? navigationManager { get; set; }

        [Inject]
        private UserService? userService { get; set; }

        [Inject]
        private UserState? userState { get; set; }

        private void SetIdentity(IIdentity newIdentity) => identity = newIdentity;
        private void CheckUser()
        {
            var user = userService!.GetUser(identity!.Name!);

            if (user is null)
                navigationManager!.NavigateTo("/register");
            else
                userState!.SetCurrentUser(user);
        }

        protected override void OnInitialized()
        {
            ModalService.ModalUpdated += (_, _) => InvokeAsync(StateHasChanged);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
                CheckUser();
        }
    }
}
