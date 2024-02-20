using COM617.Data;
using COM617.Services.Identity;
using Microsoft.AspNetCore.Components;

namespace COM617.Shared
{
    public partial class LoginDisplay
    {
        private bool collapseProfileMenu { get; set; } = true;

        [Inject]
        private UserService? UserService { get; set; }

        private User? User => UserService!.CurrentUser();
    }
}
