using System.Collections.Generic;
using NoTricks.Data.Models;

namespace NoTricks.Data.Repositories {
    public interface IAuthorRepo : IRepository<Author> { }

    public class AuthorRepo : IAuthorRepo {
        private readonly string _connStr;

        public AuthorRepo(NoTricksConnectionString connStr) {
            _connStr = connStr.Value;
        }
        
        public int Insert(Author model) {
            throw new System.NotImplementedException();
        }

        public Author GetById(int id) {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Author> GetAll() {
            throw new System.NotImplementedException();
        }

        public bool Remove(int id) {
            throw new System.NotImplementedException();
        }

        public bool Update(Author model) {
            throw new System.NotImplementedException();
        }
    }
}