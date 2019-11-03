using System.Collections.Generic;

namespace NoTricks.Data.Repositories {
    public interface IMappingRepository<T> {
        IEnumerable<T> GetAll();
        bool Insert(T model);
        bool Exist(T model);
        bool Remove(T model);
    }
}