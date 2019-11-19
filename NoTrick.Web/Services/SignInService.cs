using System.Threading.Tasks;
using NoTricks.Data.Models;
using NoTricks.Data.Repositories;
using Org.BouncyCastle.Security;

namespace NoTrick.Web.Services {
    public class SignInService {
        private readonly IAccountRepo _accountRepo;

        public SignInService(IAccountRepo accountRepo) {
            _accountRepo = accountRepo;
        }
        
        public AccountStatus CheckAccount(string email, string password) {
            var account = _accountRepo.GetByEmail(email);
            if (account == null) return AccountStatus.Unauthenticated;

            var hashedPassword = PasswordHasher.HashPassword(password, account.PasswordSalt);
            
            return hashedPassword.Item1 != account.PasswordHash ? AccountStatus.Unauthenticated : account.Status;
        }

        public async Task SignIn(string email, bool rememberMe) { }
    }
}