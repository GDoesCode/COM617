using COM617.Data;
using COM617.Services;
using COM617.Services.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

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

        private async Task HandleSelectedFile(InputFileChangeEventArgs args)
        {
            var file = args.File;
            long maxAllowedSize = 10 * 1024 * 1024; // Maximum file size (10 MB)
            if (file != null)
            {
                try
                {
                    var format = "image/png";
                    var resizedImage = await file.RequestImageFileAsync(format, 640, 480); // Optional: Resize image

                    using var ms = new MemoryStream();
                    await resizedImage.OpenReadStream(maxAllowedSize).CopyToAsync(ms);
                    user.ProfilePicUrl = $"data:{format};base64,{Convert.ToBase64String(ms.ToArray())}";
                    StateHasChanged();
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error uploading image: {ex.Message}");
                }
            }
        }
    }
}
