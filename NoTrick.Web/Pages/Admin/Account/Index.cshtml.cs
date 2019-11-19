using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoTricks.Data.Repositories;

namespace NoTrick.Web.Pages.Admin.Account {
    public class Index : PageModel {
        private readonly IAccountRepo _accountRepo;

        public Index(IAccountRepo accountRepo) {
            _accountRepo = accountRepo;
        }
        
        public IEnumerable<NoTricks.Data.Models.Account> Accounts;
        
        public void OnGet() {
            Accounts = _accountRepo.GetAll();
        }
    }
}