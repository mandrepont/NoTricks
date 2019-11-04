using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;
using NoTricks.Data.Models;

namespace NoTricks.Data.Repositories {
    public interface IRoleRepo : IRepository<Role> { }
    
    public class RoleRepo : IRoleRepo {
        private readonly string _connStr;

        public RoleRepo(NoTricksConnectionString connStr) {
            _connStr = connStr.Value;
        }
        
        public int Insert(Role model) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                START TRANSACTION;
                INSERT INTO Roles(Name, Description)
                VALUES (@{nameof(Role.Name)}, @{nameof(Role.Description)});
                SELECT @@IDENTITY;
                COMMIT;
            ";
            return conn.Query<int>(sql, model).Single();
        }

        public Role GetById(int id) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
               SELECT
                 Id AS {nameof(Role.Id)},
                 Name AS {nameof(Role.Name)},
                 Description AS {nameof(Role.Description)}
               FROM Roles
               WHERE Id = @Id;
            ";
            return conn.QuerySingleOrDefault<Role>(sql, new {Id = id});
        }

        public IEnumerable<Role> GetAll() {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
               SELECT
                 Id AS {nameof(Role.Id)},
                 Name AS {nameof(Role.Name)},
                 Description AS {nameof(Role.Description)}
               FROM Roles;
            ";
            return conn.Query<Role>(sql);
        }

        public bool Remove(int id) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = @"
                DELETE FROM Roles WHERE Id = @Id 
            ";
            return conn.Execute(sql, new {id}) == 1;
        }

        public bool Update(Role model) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
              UPDATE Roles SET
                Name = @{nameof(Role.Name)},
                Description = @{nameof(Role.Description)}
              WHERE Id = @{nameof(Role.Id)};
            ";
            return conn.Execute(sql, model) == 1;
        }
    }
}
