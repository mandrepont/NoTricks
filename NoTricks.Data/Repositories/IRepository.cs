using System.Collections.Generic;

namespace NoTricks.Data.Repositories {
    public interface IRepository<T> {
        int Insert(T model);
        T GetById(int id);
        IEnumerable<T> GetAll();
        bool Remove(int id);
        bool Update(T model);
    }
}