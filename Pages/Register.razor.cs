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
        private SignInService? SignInService { get; set; }

        private async void RequestAccount(IIdentity identity)
        {
            var userApp = UserService!.GetUserApplication(identity.Name!);
            if (userApp is not null)
            {
                accountRequested = true;
            }
            else
            {
                var newUser = new User(identity.Name!, "Test", "Test");
                await UserService.CreateUserApplication(new UserApplication(newUser));
                accountRequested = true;
            }
            StateHasChanged();
        }

        private async Task CreateAccount(IIdentity identity)
        {
            var user = new User(identity.Name!, "Test", "Test", UserRole.Admin);
            accountCreated = await UserService!.CreateUser(user);
            await SignInService!.PasswordSignInAsync("", "");
            StateHasChanged();
        }
    }
}