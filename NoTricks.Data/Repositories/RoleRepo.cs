using System.Collections.Generic;
using NoTricks.Data.Models;

namespace NoTricks.Data.Repositories {
    public interface IRoleRepo : IRepository<Role> { }
    
    public class RoleRepo : IRoleRepo {
        public int Insert(Role model) {
            throw new System.NotImplementedException();
        }

        public Role GetById(int id) {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Role> GetAll() {
            throw new System.NotImplementedException();
        }

        public bool Remove(int id) {
            throw new System.NotImplementedException();
        }

        public bool Update(Role model) {
            throw new System.NotImplementedException();
        }
    }
}