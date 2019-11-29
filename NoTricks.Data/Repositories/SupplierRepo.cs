using System.Collections.Generic;
using NoTricks.Data.Models;

namespace NoTricks.Data.Repositories {
    public interface ISupplierRepo : IRepository<Supplier>{}
    
    public class SupplierRepo : ISupplierRepo {
        private readonly string _connStr;

        public SupplierRepo(NoTricksConnectionString connStr) {
            _connStr = connStr.Value;
        }
        
        public int Insert(Supplier model) {
            throw new System.NotImplementedException();
        }

        public Supplier GetById(int id) {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Supplier> GetAll() {
            throw new System.NotImplementedException();
        }

        public bool Remove(int id) {
            throw new System.NotImplementedException();
        }

        public bool Update(Supplier model) {
            throw new System.NotImplementedException();
        }
    }
}