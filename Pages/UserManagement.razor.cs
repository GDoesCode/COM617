using COM617.Data;
using COM617.Services;
using COM617.Services.Identity;
using Microsoft.AspNetCore.Components;
using System.Drawing.Text;

namespace COM617.Pages
{
    public partial class UserManagement
    {
        public List<User> Users { get; set; } = new List<User>();

        [Inject]
        private UserService? UserService { get; set; }

        [Inject]
        private ModalService? ModalService { get; set; }

        protected override void OnInitialized()
        {
            async void AddUser(object? _, User user)
            {
                Users.Add(user);
                await InvokeAsync(StateHasChanged);
            }
            async void RemoveUser(object? _, User user)
            {
                Users.Remove(user);
                await InvokeAsync(StateHasChanged);
            }
            async void UpdateUser(object? _, User user)
            {
                Users.RemoveAll(u => u.Id == user.Id);
                Users.Add(user);
                await InvokeAsync(StateHasChanged);
            }

            Users = UserService!.GetUsers();
            UserService.OnUserCreated += AddUser;
            UserService.OnUserUpdated += UpdateUser;
            UserService.OnUserDeleted += RemoveUser;
        }
    }
}
