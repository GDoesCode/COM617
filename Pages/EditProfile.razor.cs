using COM617.Data;
using COM617.Services.Identity;
using Microsoft.AspNetCore.Components;

namespace COM617.Pages
{
    public partial class EditProfile
    {
        [Inject]
        private UserState? UserState { get; set; }

        [Inject]
        private NavigationManager? NavigationManager { get; set; }

        private User User => UserState!.CurrentUser!;

        protected override void OnInitialized()
        {
            if (UserState!.CurrentUser is null)
            {
                NavigationManager!.NavigateTo("/EditProfile");
            }
        }

        private async Task<bool> Save() => await UserState!.Save();
    }
}
