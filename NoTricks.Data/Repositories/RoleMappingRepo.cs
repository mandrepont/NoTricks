using System.Collections.Generic;
using Dapper;
using MySql.Data.MySqlClient;
using NoTricks.Data.Models;

namespace NoTricks.Data.Repositories {
    public interface IRoleMappingRepo : IMappingRepository<RoleMapping> { }
    
    public class RoleMappingRepo : IRoleMappingRepo {
        private readonly string _connStr;

        public RoleMappingRepo(NoTricksConnectionString connStr) {
            _connStr = connStr.Value;
        }
        
        public bool Insert(RoleMapping model) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                INSERT INTO RoleMappings(RoleId, AccountId)
                VALUES (@{nameof(RoleMapping.RoleId)}, @{nameof(RoleMapping.AccountId)});
            ";
            return conn.Execute(sql, model) == 1;
        }

        public bool Exist(RoleMapping model) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                SELECT COUNT(1) FROM RoleMappings
                WHERE
                  RoleId = @{nameof(RoleMapping.RoleId)} AND AccountId = @{nameof(RoleMapping.AccountId)};
            ";
            return conn.ExecuteScalar<bool>(sql, model);
        }

        public bool Remove(RoleMapping model) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                DELETE FROM RoleMappings
                WHERE
                  RoleId = @{nameof(RoleMapping.RoleId)} AND AccountId = @{nameof(RoleMapping.AccountId)};
            ";
            return conn.Execute(sql, model) == 1;
        }

        public IEnumerable<RoleMapping> GetAll() {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
               SELECT
                 RoleId AS {nameof(RoleMapping.RoleId)},
                 AccountId AS {nameof(RoleMapping.AccountId)}
               FROM RoleMappings
            ";
            return conn.Query<RoleMapping>(sql);
        }
    }
}