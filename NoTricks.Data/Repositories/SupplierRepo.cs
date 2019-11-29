using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;
using NoTricks.Data.Models;

namespace NoTricks.Data.Repositories {
    public interface ISupplierRepo : IRepository<Supplier>{}
    
    public class SupplierRepo : ISupplierRepo {
        private readonly string _connStr;

        public SupplierRepo(NoTricksConnectionString connStr) {
            _connStr = connStr.Value;
        }
        
        public int Insert(Supplier model) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                START TRANSACTION;
                INSERT INTO Suppliers (CompanyName, Balance, ManagerId, AddressId)
                VALUES(
                    @{nameof(Supplier.CompanyName)}, @{nameof(Supplier.Balance)}, 
                    @{nameof(Supplier.ManagerId)}, @{nameof(Supplier.AddressId)}
                );
                SELECT @@IDENTITY;
                COMMIT;
            ";

            return conn.Query<int>(sql, model).Single();
        }

        public Supplier GetById(int id) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                    SELECT 
                      Id AS {nameof(Supplier.Id)},
                      CompanyName AS {nameof(Supplier.CompanyName)},
                      Balance AS {nameof(Supplier.Balance)},
                      ManagerId AS {nameof(Supplier.ManagerId)},
                      AddressId AS {nameof(Supplier.AddressId)}
                    FROM Suppliers
                    WHERE Id = @Id;
                ";
            return conn.QuerySingleOrDefault<Supplier>(sql, new {Id = id});
        }

        public IEnumerable<Supplier> GetAll() {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                    SELECT 
                      Id AS {nameof(Supplier.Id)},
                      CompanyName AS {nameof(Supplier.CompanyName)},
                      Balance AS {nameof(Supplier.Balance)},
                      ManagerId AS {nameof(Supplier.ManagerId)},
                      AddressId AS {nameof(Supplier.AddressId)}
                    FROM Suppliers;                   
                ";
            return conn.Query<Supplier>(sql);  
        }

        public bool Remove(int id) {
            using var conn = new MySqlConnection(_connStr); 
            conn.Open();
            var sql = @"
                DELETE FROM Suppliers WHERE Id = @Id
            ";
            return conn.Execute(sql, new {id}) == 1;
        }

        public bool Update(Supplier model) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                UPDATE Suppliers SET
                  CompanyName = @{nameof(Supplier.CompanyName)},
                  Balance = @{nameof(Supplier.Balance)},
                  ManagerId = @{nameof(Supplier.ManagerId)},
                  AddressId = @{nameof(Supplier.AddressId)}
                WHERE Id = @{nameof(Supplier.Id)};
            ";
            return conn.Execute(sql, model) == 1;
        }
    }
}