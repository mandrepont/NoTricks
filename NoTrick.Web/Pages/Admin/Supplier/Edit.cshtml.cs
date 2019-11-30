using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoTricks.Data.Repositories;

namespace NoTrick.Web.Pages.Admin.Supplier {
    public class Edit : PageModel {
        private readonly ISupplierRepo _supplierRepo;
        public int? Id;

        public Edit(ISupplierRepo supplierRepo) {
            _supplierRepo = supplierRepo;
        }
        [BindProperty] public InputModel Input { get; set; }
        
        public class InputModel {
            [Required]
            [Display(Name = "Company Name")]
            public string CompanyName { get; set; }

            [Required]
            [Display(Name = "Balance")]
            public decimal Balance { get; set; }
        }
        
        public void OnGet(int? id) {
            Id = id;
            if (id.HasValue) {
                var supplier = _supplierRepo.GetById(id.Value);
                Input = new InputModel {
                    CompanyName = supplier.CompanyName,
                    Balance = supplier.Balance
                }; 
            }
            else {
                Input = new InputModel();
            }
        }

        public IActionResult OnPost(int? id) {
            //input model to supplier
            //Update in repo
            if (id.HasValue) {
                var supplier = _supplierRepo.GetById(id.Value);
                supplier.CompanyName = Input.CompanyName;
                supplier.Balance = Input.Balance;
                _supplierRepo.Update(supplier);
            }
            else {
                if (!ModelState.IsValid) return Page();
                
                var supplier = new NoTricks.Data.Models.Supplier {
                    CompanyName = Input.CompanyName,
                    Balance = Input.Balance
                };

                _supplierRepo.Insert(supplier);
            }

            return RedirectToPage("Index");
        }
    }
}