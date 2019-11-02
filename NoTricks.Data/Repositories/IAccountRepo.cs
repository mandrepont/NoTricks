using NoTricks.Data.Models;

namespace NoTricks.Data.Repositories {
    public interface IAccountRepo : IRepository<Account> {
        Account GetByEmail(string email);
    }
}