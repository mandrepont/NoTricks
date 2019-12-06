using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NoTricks.Data.Models;
using NoTricks.Data.Repositories;

namespace NoTrick.Web.Pages.Admin.Account {
    public class Edit : PageModel {
        private readonly IAccountRepo _accountRepo;
        private readonly IProfileRepo _profileRepo;
        private readonly IRoleRepo _roleRepo;
        private readonly IRoleMappingRepo _roleMappingRepo;
        
        public NoTricks.Data.Models.Account Account { get; set; }
        public Profile Profile { get; set; }
        public IEnumerable<Role> UserRoles { get; set; }
        public IEnumerable<SelectListItem> AvailableRoles { get; set; }
        
        [BindProperty] 
        public InputModel Input { get; set; }
        
        [BindProperty] 
        public SecurityInputModel SecurityInput { get; set; }
        
        public Edit(IAccountRepo accountRepo,
                    IProfileRepo profileRepo,
                    IRoleRepo roleRepo,
                    IRoleMappingRepo roleMappingRepo) {
            _accountRepo = accountRepo;
            _profileRepo = profileRepo;
            _roleRepo = roleRepo;
            _roleMappingRepo = roleMappingRepo;
        }
        
        public class SecurityInputModel {
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
        
        public class InputModel {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            
            [Required]
            [Display(Name = "Account Status")]
            [EnumDataType(typeof(AccountStatus))]
            public AccountStatus Status { get; set; }
            
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Name = "Perferred Name")]
            public string PerferredName { get; set; }
            
            [Display(Name = "Phone Number")]
            [DataType(DataType.PhoneNumber)]
            public string Phone { get; set; }

            [Display(Name = "Date Of Birth")]
            [DataType(DataType.Date)]
            public DateTime? DateOfBirth { get; set; }
        }
        
        public IActionResult OnGet(int id) {
            Account = _accountRepo.GetById(id);
            Profile = _profileRepo.GetByAccountId(id);
            if (Account == null || Profile == null) return NotFound();

            UserRoles = _roleRepo.GetAllFormAccountId(id);
            AvailableRoles = _roleRepo.GetAll()
                                      .Except(UserRoles)
                                      .Select(r => new SelectListItem(r.Name, r.Id.ToString()));
            
            Input = new InputModel {
                DateOfBirth = Profile.Birthday,
                FirstName = Profile.FirstName,
                LastName = Profile.LastName,
                PerferredName = Profile.PreferredName,
                Phone = Profile.Phone,
                Email = Account.EMail,
                Status = Account.Status
            };
            return Page();
        }

        public IActionResult OnPostRole(int id, [FromForm] int newRoleId) {
            var mapping = new RoleMapping {
                AccountId = id,
                RoleId = newRoleId
            };
            _roleMappingRepo.Insert(mapping);
            return RedirectToPage();
        }

        public IActionResult OnGetRole(int id, int roleId) {
            var mapping = new RoleMapping {
                AccountId = id,
                RoleId = roleId
            };
            _roleMappingRepo.Remove(mapping);
            return RedirectToPage();
        }

        public IActionResult OnPostSecurity(int id) {
            Account = _accountRepo.GetById(id);
            if (Account == null) return NotFound();
            
            //TODO: Model validation
            var (passwordHash, passwordSalt) = PasswordHasher.HashPassword(SecurityInput.Password);
            Account.PasswordHash = passwordHash;
            Account.PasswordSalt = passwordSalt;
            Account.LastModifiedAt = DateTime.UtcNow;
            _accountRepo.Update(Account);
            
            //TODO: Add success message
            return RedirectToPage();
        }
        
        public IActionResult OnPostGeneral(int id) {
            Account = _accountRepo.GetById(id);
            Profile = _profileRepo.GetByAccountId(id);
            if (Account == null || Profile == null) return NotFound();

            Account.EMail = Input.Email;
            Account.Status = Input.Status;
            Account.LastModifiedAt = DateTime.UtcNow;
            
            Profile.Birthday = Input.DateOfBirth;
            Profile.Phone = Input.Phone;
            Profile.FirstName = Input.FirstName;
            Profile.PreferredName = Input.PerferredName;
            Profile.LastName = Input.LastName;

            _accountRepo.Update(Account);
            _profileRepo.Update(Profile);
            
            //TODO: Add success message
            return RedirectToPage();
        }

    }
}