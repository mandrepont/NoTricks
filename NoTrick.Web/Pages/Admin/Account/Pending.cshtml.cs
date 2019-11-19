using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoTricks.Data.Models;
using NoTricks.Data.Repositories;

namespace NoTrick.Web.Pages.Admin.Account {
    public class Pending : PageModel
    {
        private readonly IAccountRepo _accountRepo;
        public IEnumerable<NoTricks.Data.Models.Account> PendingAccounts;

        public Pending(IAccountRepo accountRepo) {
            _accountRepo = accountRepo;
        }
        
        public void OnGet() {
            PendingAccounts = _accountRepo.GetPending();
        }

        public IActionResult OnGetUpdateStatus(int id, bool approve) {
            var account = _accountRepo.GetById(id);
            //TODO: Add error message here.
            if (account == null) return NotFound();
            
            account.Status = approve ? AccountStatus.Ok : AccountStatus.Disabled;
            _accountRepo.Update(account);
            return RedirectToPage();
        }
    }
}