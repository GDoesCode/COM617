using COM617.Components.Nav;
using COM617.Services;
using Microsoft.AspNetCore.Components;

namespace COM617.Shared
{
    public partial class MainLayout
    {
        [Inject]
        private MongoDbService? mongoDbService { get; set; }

        [Inject]
        private NavigationManager? navigationManager { get; set; }

        private MobileNavbar? mobileNavbarRef { get; set; }

        /*
        protected override void OnInitialized()
        {
            if (!UserRegistered())
                navigationManager!.NavigateTo("unregistered", true);
        }*/

        //private bool UserRegistered() => mongoDbService!.GetDocumentsByFilter<User>(user => user.Email == identityService!.CurrentUser().Email).Any();
    }
}
