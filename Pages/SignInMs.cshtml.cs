using COM617.Services;
using COM617.Services.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace COM617.Pages
{
    public class SignInMsModel : PageModel
    {
        private readonly SignInService signInService;
        private readonly UserService userService;

        public SignInMsModel(SignInService signInService, UserService userService)
        {
            this.signInService = signInService;
            this.userService = userService;
        }
        

        public async Task<IActionResult> OnPostAsync(string returnUrl = null!)
        {
            var mongoUser = await userService.GetUser(HttpContext.User.Identity!.Name!);
            if (mongoUser != null)
            {
                //await signInService.SignOutAsync(HttpContext.User);
                await signInService.SignInAsync(mongoUser);
                HttpContext.Response.Redirect("/index");
            }
            else
            {
                HttpContext.Response.Redirect("/register");
            }

            return Page();
        }
    }
}
