using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoTricks.Data.Repositories;

namespace NoTrick.Web.Pages.Admin.Supplier {
    public class Index : PageModel {
        private readonly ISupplierRepo _supplierRepo;
        public IEnumerable<NoTricks.Data.Models.Supplier> Suppliers;

        public Index(ISupplierRepo supplierRepo) {
            _supplierRepo = supplierRepo;
        }
        
        public void OnGet() {
            Suppliers = _supplierRepo.GetAll();
        }

        public IActionResult OnGetRemove(int id) {
            //TODO: Handle false case
            _supplierRepo.Remove(id);
            return RedirectToPage();
        }
    }
}