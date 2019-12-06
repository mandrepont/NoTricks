using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace NoTrick.Web.Pages.Account {
    public class LogoutModel : PageModel {
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(ILogger<LogoutModel> logger) {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet() {
            if (!User.Identity.IsAuthenticated) return Page();
            
            _logger.LogInformation($"Logging {User.Identity.Name} out.");
            await HttpContext.SignOutAsync();
            //Do a redirect so that the username is cleared from the layout.
            return RedirectToPage();
        }
    }
}