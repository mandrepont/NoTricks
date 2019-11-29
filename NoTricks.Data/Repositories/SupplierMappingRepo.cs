using System.Collections.Generic;
using NoTricks.Data.Models;

namespace NoTricks.Data.Repositories {
    public interface ISupplierMappingRepo : IMappingRepository<SupplierMapping> { }
    
    public class SupplierMappingRepo : ISupplierMappingRepo {
        private readonly string _connStr;
        
        public SupplierMappingRepo(NoTricksConnectionString connStr) {
            _connStr = connStr.Value;
        }
        
        public IEnumerable<SupplierMapping> GetAll() {
            throw new System.NotImplementedException();
        }

        public bool Insert(SupplierMapping model) {
            throw new System.NotImplementedException();
        }

        public bool Exist(SupplierMapping model) {
            throw new System.NotImplementedException();
        }

        public bool Remove(SupplierMapping model) {
            throw new System.NotImplementedException();
        }
    }
}