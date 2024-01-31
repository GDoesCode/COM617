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

        [Inject]
        private NavigationManager? navigationManager { get; set; }

        [Inject]
        private UserService? userService { get; set; }

        [Inject]
        private UserState? userState { get; set; }

        private async void CheckUser(IIdentity identity)
        {
            var user = userService!.GetUser(identity.Name!);
            if (user is null)
            {
                await userService.CreateUser(new User
                {
                    Email = identity.Name!,
                    Firstname = identity.Name!,
                    Lastname = identity.Name!,
                    Role = UserRole.Admin
                });
            } 
            else
            {
                userState!.SetCurrentUser(user);
            }
        }
    }
}
