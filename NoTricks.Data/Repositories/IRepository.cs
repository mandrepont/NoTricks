namespace NoTricks.Data.Repositories {
    public interface IRepository<T> {
        int Insert(T model);
        T GetById(int id);
        bool Remove(int id);
        bool Update(T model);
    }
}