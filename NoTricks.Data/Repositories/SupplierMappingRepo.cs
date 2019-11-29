using System.Collections.Generic;
using Dapper;
using MySql.Data.MySqlClient;
using NoTricks.Data.Models;

namespace NoTricks.Data.Repositories {
    public interface ISupplierMappingRepo : IMappingRepository<SupplierMapping> { }
    
    public class SupplierMappingRepo : ISupplierMappingRepo {
        private readonly string _connStr;
        
        public SupplierMappingRepo(NoTricksConnectionString connStr) {
            _connStr = connStr.Value;
        }
        
        public bool Insert(SupplierMapping model) {
            //Do not insert existing mapping, it throws exceptions.
            if (Exist(model)) return false;
            
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                INSERT INTO SupplierMappings(SupplierId, AccountId)
                VALUES (@{nameof(SupplierMapping.SupplierId)}, @{nameof(SupplierMapping.AccountId)});
            ";
            return conn.Execute(sql, model) == 1;
        }

        public bool Exist(SupplierMapping model) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                SELECT COUNT(1) FROM SupplierMappings
                WHERE
                  SupplierId = @{nameof(SupplierMapping.SupplierId)} AND AccountId = @{nameof(SupplierMapping.AccountId)};
            ";
            return conn.ExecuteScalar<bool>(sql, model);
        }

        public bool Remove(SupplierMapping model) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                DELETE FROM SupplierMappings
                WHERE
                  SupplierId = @{nameof(SupplierMapping.SupplierId)} AND AccountId = @{nameof(SupplierMapping.AccountId)};
            ";
            return conn.Execute(sql, model) == 1;
        }
        
        public IEnumerable<SupplierMapping> GetAll() {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
               SELECT
                 SupplierId AS {nameof(SupplierMapping.SupplierId)},
                 AccountId AS {nameof(SupplierMapping.AccountId)}
               FROM SupplierMappings
            ";
            return conn.Query<SupplierMapping>(sql);
        }

    }
}