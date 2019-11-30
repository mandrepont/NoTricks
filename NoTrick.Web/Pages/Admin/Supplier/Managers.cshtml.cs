using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoTrick.Web.Pages.Supplier;
using NoTricks.Data.Models;
using NoTricks.Data.Repositories;

namespace NoTrick.Web.Pages.Admin.Supplier {
    public class Managers : PageModel {
        public NoTricks.Data.Models.Supplier Supplier { get; set; }
        public IEnumerable<SupplierManager> SupplierManager { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int ManagerId { get; set; }
        
        private readonly ISupplierRepo _supplierRepo;
        private readonly ISupplierMappingRepo _mappingRepo;
        
        public Managers(ISupplierRepo supplierRepo, ISupplierMappingRepo mappingRepo) {
            _supplierRepo = supplierRepo;
            _mappingRepo = mappingRepo;
        }
        
        public void OnGet(int id) {
            Supplier = _supplierRepo.GetById(id);
            SupplierManager = _mappingRepo.GetSupplierManager(id);
        }

        public IActionResult OnGetRemove(int id, int managerId) {
            var supplierMapping = new SupplierMapping {SupplierId = id, AccountId = managerId};
            _mappingRepo.Remove(supplierMapping);
            return RedirectToPage();
        }

        public IActionResult OnPostAdd(int id) {
            var supplierMapping = new SupplierMapping {SupplierId = id, AccountId = ManagerId};
            _mappingRepo.Insert(supplierMapping);
            return RedirectToPage();
        }
    }
}