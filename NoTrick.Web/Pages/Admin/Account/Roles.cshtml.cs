using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoTricks.Data.Models;
using NoTricks.Data.Repositories;

namespace NoTrick.Web.Pages.Admin.Account {
    public class RolesModel : PageModel {
        private readonly IRoleRepo _roleRepo;
        public IEnumerable<Role> Roles { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public RolesModel(IRoleRepo roleRepo) {
            _roleRepo = roleRepo;
        }

        public void OnGet() {
            Roles = _roleRepo.GetAll();
        }

        public IActionResult OnGetRemove(int id) {
            //TODO: Determin if we should remove all role mappings. 
            _roleRepo.Remove(id);
            return RedirectToPage();
        }

        public class InputModel {
            [Required]
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public IActionResult OnPost() {
            if (!ModelState.IsValid)
                return Page();

            //TODO: Need some more input validation here. EX Role name should be UQ
            var role = new Role {
                Name = Input.Name,
                Description = Input.Description
            };
            _roleRepo.Insert(role);

            return RedirectToPage();
        }
    }
}