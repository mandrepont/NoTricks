using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using NoTricks.Data.Models;
using Dapper;

namespace NoTricks.Data.Repositories {
    public interface IAuthorRepo : IRepository<Author> { }

    public class AuthorRepo : IAuthorRepo {
        private readonly string _connStr;

        public AuthorRepo(NoTricksConnectionString connStr) {
            _connStr = connStr.Value;
        }
        
        public int Insert(Author model) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                START TRANSACTION;
                INSERT INTO Authors(FirstName, LastName, PenName, Birthday)
                VALUES (
                    @{nameof(Author.FirstName)}, @{nameof(Author.LastName)},
                    @{nameof(Author.PenName)}, @{nameof(Author.Birthday)}
                );
                SELECT @@IDENTITY;
                COMMIT;
            ";
            
            return conn.Query<int>(sql, model).Single();
        }

        public Author GetById(int id) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                    SELECT 
                      Id AS {nameof(Author.Id)},
                      FirstName AS {nameof(Author.FirstName)},
                      LastName AS {nameof(Author.LastName)},
                      PenName AS {nameof(Author.PenName)},
                      Birthday AS {nameof(Author.Birthday)}
                    FROM Authors
                    WHERE Id = @Id;
                ";
            return conn.QuerySingleOrDefault<Author>(sql, new {Id = id});
        }

        public IEnumerable<Author> GetAll() {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                  SELECT 
                    Id AS {nameof(Author.Id)},
                    FirstName AS {nameof(Author.FirstName)},
                    LastName AS {nameof(Author.LastName)},
                    PenName AS {nameof(Author.PenName)},
                    Birthday AS {nameof(Author.Birthday)}
                  FROM Authors;          
            ";
            return conn.Query<Author>(sql);
        }

        public bool Remove(int id) {
            using var conn = new MySqlConnection(_connStr); 
            conn.Open();
            var sql = @"
                DELETE FROM Author WHERE Id = @Id
            ";
            return conn.Execute(sql, new {id}) == 1;
        }

        public bool Update(Author model) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                UPDATE Authors SET
                  FirstName = @{nameof(Author.FirstName)},
                  LastName = @{nameof(Author.LastName)},
                  PenName = @{nameof(Author.PenName)},
                  Birthday = @{nameof(Author.Birthday)}
                WHERE Id = @{nameof(Author.Id)};
            ";
            return conn.Execute(sql, model) == 1;
        }
    }
}