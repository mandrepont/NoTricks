using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NoTricks.Data.Models;
using NoTricks.Data.Repositories;

namespace NoTrick.Web.Pages.Admin {
    public class PayoutsModel : PageModel {
        private readonly ILogger<PayoutsModel> _logger;
        private readonly ISupplierPayoutRepo _supplierPayoutRepo;
        private readonly ISupplierRepo _supplierRepo;

        public IEnumerable<ExtraSupplierPayout> PayoutHistory { get; set; }
        public IEnumerable<NoTricks.Data.Models.Supplier> PendingPayouts { get; set; }

        public PayoutsModel(ILogger<PayoutsModel> logger,
                            ISupplierPayoutRepo supplierPayoutRepo,
                            ISupplierRepo supplierRepo) {
            _logger = logger;
            _supplierPayoutRepo = supplierPayoutRepo;
            _supplierRepo = supplierRepo;
        }

        public void OnGet() {
            PayoutHistory = _supplierPayoutRepo.GetAllExtra();
            PendingPayouts = _supplierRepo.GetAll().Where(x => x.Balance > 0);
        }

        public IActionResult OnGetPayout(int supplierId) {
            var supplier = _supplierRepo.GetById(supplierId);
            if (supplier == null) return NotFound();
            
            //Create payout and zero out supplier balance
            var payout = new SupplierPayout {
                Amount = supplier.Balance,
                PayedAt = DateTime.UtcNow,
                StaffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                SupplierId = supplier.Id
            };
            supplier.Balance = 0;
            
            //This should be in a tranaction, but we need to ensure the payment is added before we update the balance.
            if(_supplierPayoutRepo.Insert(payout) != 0) 
                _supplierRepo.Update(supplier);
           
            return RedirectToPage();
        }
    }

}
