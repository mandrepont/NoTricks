namespace NoTricks.Data.Repositories {
    public interface IRepository<T> {
        T Insert(T model);
        T GetById(int id);
        bool Remove(T model);
        bool Update(T model);
    }
}