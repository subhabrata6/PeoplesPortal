using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TestEmployeeApp.Model;
using TestEmployeeApp.DBAccess;
using Microsoft.AspNetCore.Http;

namespace TestEmployeeApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;
        [BindProperty]
        public AccountModel Account { get; set; }
        public string ErrorMessage { get; set; }
        public string ReturnUrl { get; set; }

        //public void OnGet()
        //{
        //    Account = new AccountModel { UserName = "Test" };
        //}

        public LoginModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            Account = new AccountModel { UserName = "Test" };
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            // Clear the existing external cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            ModelState.Clear();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {

                ResponseMessage user = new AccountDbAccess(_configuration).AuthenticateUser(Account);

                if (user.Status != System.Net.HttpStatusCode.OK)
                {
                    TempData["AlertMessage"] = user.Message;
                    return Page();
                }
                TempData["AlertMessage"] = null;

                var userRole = (AssignRole)user.Response;

                var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Email, Account.Email),
                                new Claim(ClaimTypes.Name, userRole.UserName),
                                new Claim(ClaimTypes.Role, userRole.RoleName),
                                new Claim(ClaimTypes.NameIdentifier, userRole.UserId.ToString())
                            };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    // Refreshing the authentication session should be allowed.

                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };
                HttpContext.User.AddIdentity(claimsIdentity);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                //
                //Response.Cookies.Append("LoginCookie", "cookieValue", cookieOptions);

                return LocalRedirect("/Index");
            }

            // Something failed. Redisplay the form.
            return Page();
        }
    }
}
