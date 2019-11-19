using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NoTrick.Web.Services;
using NoTricks.Data.Models;
using NoTricks.Data.Repositories;

namespace NoTrick.Web.Pages.Account {
    [AllowAnonymous]
    public class LoginModel : PageModel {
        private readonly ILogger<LoginModel> _logger;
        private readonly IAccountRepo _account;
        private readonly SignInService _signInService;

        public LoginModel(ILogger<LoginModel> logger, IAccountRepo account, SignInService signInService) {
            _logger = logger;
            _account = account;
            _signInService = signInService;
        }

        [BindProperty] public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData] public string ErrorMessage { get; set; }

        public class InputModel {
            [Required] [EmailAddress] public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")] public bool RememberMe { get; set; }
            
        }

        public async Task OnGetAsync(string returnUrl = null) {
            if (!string.IsNullOrEmpty(ErrorMessage)) {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync();

            ReturnUrl = returnUrl;
        }
        
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid) {
                //Need to check account.
                //if (result is invalid)
                switch (_signInService.CheckAccount(Input.Email, Input.Password)) {
                    case AccountStatus.Banned:
                        ModelState.AddModelError(string.Empty, "Your account has been banned.");
                        return Page();
                    case AccountStatus.Disabled:
                        ModelState.AddModelError(string.Empty, "Your account has been disabled.");
                        return Page();
                    case AccountStatus.Unauthenticated:
                        ModelState.AddModelError(string.Empty, "Invalid credentials.");
                        return Page();
                    case AccountStatus.LockedOut:
                        ModelState.AddModelError(string.Empty, "Account locked.");
                        return Page();
                    case AccountStatus.PendingVerification:
                        ModelState.AddModelError(string.Empty, "Your account is pending verification.");
                        return Page();
                    case AccountStatus.Ok:
                        await _signInService.SignInAsync(Input.Email, Input.RememberMe, HttpContext);
                        return LocalRedirect(returnUrl);
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError(string.Empty, "An unexpected error occured.");
            return Page();
        }

        public async Task<IActionResult> OnGetLogoutAsync() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect(Url.Content("~/"));
        }

        
    }

}