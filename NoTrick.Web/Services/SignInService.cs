using System.Threading.Tasks;
using NoTricks.Data.Models;
using NoTricks.Data.Repositories;

namespace NoTrick.Web.Services {
    public class SignInService {
        private IAccountRepo _accountRepo;
        public AccountStatus CheckAccount(string email, string password) {
            var account = _accountRepo.GetByEmail(email);
            if (account == null) return AccountStatus.Unauthenticated;

            return account.Status;
        }

        public async Task SignIn(string email, bool rememberMe) {
            
        }
    }
}