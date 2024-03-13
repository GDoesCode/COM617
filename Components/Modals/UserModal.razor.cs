using COM617.Data;
using COM617.Services;
using COM617.Services.Identity;
using Microsoft.AspNetCore.Components;

namespace COM617.Components.Modals
{
    public partial class UserModal
    {
        private User user = null!;

        [Inject]
        private ModalService? ModalService { get; set; }

        [Inject]
        private UserService? UserService { get; set; }

        private bool Mode => ModalService!.DataIn is not null;

        protected override void OnInitialized()
        {
            if (Mode)
                user = (User)ModalService!.DataIn!;
            else
                user = new User("", UserRole.Standard);
        }

        private async void Save()
        {
            if (Mode)
                await UserService!.UpdateUser(user);
            else
                await UserService!.CreateUser(user);

            ModalService!.Finish(user);
        }
    }
}
