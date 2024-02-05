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
        private UserService? UserService { get; set; }

        [Inject]
        private UserState? UserState { get; set; }

        private async void RequestAccount(IIdentity identity)
        {
            var userApp = UserService!.GetUserApplication(identity.Name!);
            if (userApp is not null)
            {
                accountRequested = true;
            }
            else
            {
                var newUser = new User(identity.Name!);
                await UserService.CreateUserApplication(new UserApplication(newUser));
                accountRequested = true;
            }
            StateHasChanged();
        }

        private async void CreateAccount(IIdentity identity)
        {
            var user = new User(identity.Name!, UserRole.Admin);
            accountCreated = await UserService!.CreateUser(user);
            UserState!.SetCurrentUser(user);
            StateHasChanged();
        }
    }
}