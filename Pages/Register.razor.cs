using COM617.Data;
using COM617.Services.Identity;
using Microsoft.AspNetCore.Components;
using System.Security.Principal;

namespace COM617.Pages
{
    public partial class Register
    {
        private bool accountRequested = false;
        private bool? accountCreated = null;

        [Inject]
        private UserService? userService { get; set; }

        private async void RequestAccount(IIdentity identity)
        {
            var userApp = userService!.GetUserApplication(identity.Name!);
            if (userApp is not null)
            {
                accountRequested = true;
            }
            else
            {
                var newUser = new User(identity.Name!, identity.Name!, identity.Name!);
                await userService.CreateUserApplication(new UserApplication(newUser));
                accountRequested = true;
            }
            StateHasChanged();
        }

        private async void CreateAccount(IIdentity identity)
        {
            accountCreated = await userService!.CreateUser(new User(identity.Name!, identity.Name!, identity.Name!));
            StateHasChanged();
        }
    }
}
