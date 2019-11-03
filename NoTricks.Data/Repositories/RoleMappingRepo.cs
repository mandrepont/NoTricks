using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;
using NoTricks.Data.Models;

namespace NoTricks.Data.Repositories {
    public interface IRoleMappingRepo : IRepository<RoleMapping> {}
    public class RoleMappingRepo : IRoleMappingRepo {
        private readonly string _connStr;

        public RoleMappingRepo(NoTricksConnectionString connStr) {
            _connStr = connStr.Value;
        }
        
        public int Insert(RoleMapping model) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                START TRANSACTION;
                INSERT INTO RoleMappings(RoleId, AccountId)
                VALUES (@{nameof(RoleMapping.RoleId)}, @{nameof(RoleMapping.AccountId)});
                SELECT @@IDENTITY;
                COMMIT;
            ";
            return conn.Query<int>(sql, model).Single();
        }

        public RoleMapping GetById(int id) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
               SELECT
                 Id AS {nameof(RoleMapping.Id)},
                 RoleId AS {nameof(RoleMapping.RoleId)},
                 AccountId AS {nameof(RoleMapping.AccountId)}
               FROM RoleMappings
               WHERE Id = @Id
            ";
            return conn.QuerySingleOrDefault<RoleMapping>(sql, new {Id = id});
        }

        public IEnumerable<RoleMapping> GetAll() {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
               SELECT
                 Id AS {nameof(RoleMapping.Id)},
                 RoleId AS {nameof(RoleMapping.RoleId)},
                 AccountId AS {nameof(RoleMapping.AccountId)}
               FROM RoleMappings
            ";
            return conn.Query<RoleMapping>(sql);
        }

        public bool Remove(int id) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = @"
                DELETE FROM RoleMappings WHERE Id = @Id 
            ";
            return conn.Execute(sql, new {id}) == 1;
        }

        public bool Update(RoleMapping model) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
              UPDATE RoleMappings SET
                RoleId = @{nameof(RoleMapping.RoleId)},
                AccountId = @{nameof(RoleMapping.AccountId)}
              WHERE Id = @{nameof(RoleMapping.Id)}
            ";
            return conn.Execute(sql, model) == 1;
        }
    }
}