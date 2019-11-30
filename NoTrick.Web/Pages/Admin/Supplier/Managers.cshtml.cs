using Microsoft.AspNetCore.Mvc.RazorPages;
using NoTrick.Web.Pages.Supplier;
using NoTricks.Data.Repositories;

namespace NoTrick.Web.Pages.Admin.Supplier {
    public class Managers : PageModel {
        public NoTricks.Data.Models.Supplier supplier { get; set; }
        private readonly ISupplierRepo _supplierRepo;
        public Managers(ISupplierRepo supplierRepo) {
            _supplierRepo = supplierRepo;
        }
        
        public void OnGet(int id) {
            supplier = _supplierRepo.GetById(id);
        }
    }
}