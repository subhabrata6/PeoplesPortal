using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public AccountModel Account { get; set; }

        public void OnGet()
        {
            Account =  new AccountModel { UserName = "Test" };
        }

        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Account.UserName, Account.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // Retrieve the authenticated user
                    var user = await _signInManager.UserManager.FindByNameAsync(Account.UserName);

                    // Create claims for the user (example: role claim)
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Account.UserName),
                    // Add additional claims as needed
                };

                    // Add role claim if the user belongs to a role
                    var roles = await _signInManager.UserManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    // Create identity with claims
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Sign in the user with the claims-based identity
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    // Redirect to a secure page
                    return RedirectToPage("/SecurePage");
                }
                else
                {
                    // Authentication failed, show an error message
                    ViewData["Error"] = "Invalid username or password.";
                    return Page();
                }
            }

            // Model is not valid, return the login page
            return Page();
        }
    }
}
