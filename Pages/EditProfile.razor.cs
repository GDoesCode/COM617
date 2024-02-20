using COM617.Data;
using COM617.Services.Identity;
using Microsoft.AspNetCore.Components;

namespace COM617.Pages
{
    public partial class EditProfile
    {
        [Inject]
        private NavigationManager? NavigationManager { get; set; }

        [Inject]
        private UserService? UserService { get; set; }

        private User User => UserService!.CurrentUser();
    }
}
