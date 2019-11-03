using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NoTricks.Data.Models;
using NoTricks.Data.Repositories;

namespace NoTrick.Web.Pages.Account {
    [AllowAnonymous]
    public class RegisterModel : PageModel {
        private readonly ILogger<RegisterModel> _logger;
        private readonly IAccountRepo _accountRepo;

        public RegisterModel(ILogger<RegisterModel> logger,
                             IAccountRepo accountRepo) {
            _logger = logger;
            _accountRepo = accountRepo;
        }

        [BindProperty] public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null) {
            ReturnUrl = returnUrl;
        }
        public IActionResult OnPostAsync(string returnUrl = null) {
            
            returnUrl ??= Url.Content("~/");

            //Return the page if the model is not valid. 
            if (!ModelState.IsValid) return Page();

            var (passwordHash, passwordSalt) = PasswordHasher.HashPassword(Input.Password);
            var account = new NoTricks.Data.Models.Account {
                EMail = Input.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedAt = DateTime.Now,
                Status = AccountStatus.PendingVerification
            };
            _logger.LogTrace($"Attempting to create account for {account.EMail}");

            if (_accountRepo.IsEmailInUse(account.EMail)) {
                _logger.LogWarning($"An account with {account.EMail} already exist");
                ModelState.AddModelError("All", "Email already in use. Please use forgot my password to recover.");
                return Page();
            }

            var id = _accountRepo.Insert(account);
            _logger.LogInformation($"Created account {account.EMail} with ID: {id}");
            //TODO: Login user once login service is done.
            return LocalRedirect(returnUrl);
        }
        
    }
}