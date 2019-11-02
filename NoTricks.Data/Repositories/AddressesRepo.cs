using NoTricks.Data.Models;

namespace NoTricks.Data.Repositories {
    public class AddressesRepo : IRepository<Addresses> {
        private readonly string _connStr;
        
        public AddressesRepo(NoTricksConnectionString connStr) {
            _connStr = connStr.Value;
        }
        public int Insert(Addresses model) {
            throw new System.NotImplementedException();
        }

        public Addresses GetById(int id) {
            throw new System.NotImplementedException();
        }

        public bool Remove(int id) {
            throw new System.NotImplementedException();
        }

        public bool Update(Addresses model) {
            throw new System.NotImplementedException();
        }
    }
}