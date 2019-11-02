using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;
using NoTricks.Data.Models;

namespace NoTricks.Data.Repositories {
    public class AddressesRepo : IRepository<Addresses> {
        private readonly string _connStr;
        
        public AddressesRepo(NoTricksConnectionString connStr) {
            _connStr = connStr.Value;
        }
        
        public int Insert(Addresses model) {
            using (var conn = new MySqlConnection(_connStr)) {
                conn.Open();
                var sql = $@"
                    INSERT INTO Addresses (Id, StreetAddress1, StreetAddress2, ZipCode, City, State)
                    VALUES(
                      @{nameof(Addresses.Id)}, @{nameof(Addresses.StreetAddress1)}, @{nameof(Addresses.StreetAddress2)},
                      @{nameof(Addresses.ZipCode)}, @{nameof(Addresses.City)}, @{nameof(Addresses.State)}
                    );
                    SELECT @@IDENTITY;    
                ";
                return conn.Query<int>(sql, model).Single();
            }
        }

        public Addresses GetById(int id) {
            using (var conn = new MySqlConnection(_connStr)) {
                conn.Open();
                var sql = $@"
                    SELECT
                      Id AS {nameof(Addresses.Id)},
                      StreetAddress1 AS {nameof(Addresses.StreetAddress1)},
                      StreetAddress2 AS {nameof(Addresses.StreetAddress2)},
                      ZipCode AS {nameof(Addresses.ZipCode)},
                      City AS {nameof(Addresses.City)},
                      State AS {nameof(Addresses.State)},
                    FROM Addresses
                    WHERE Id = @Id;
                ";
                
                return conn.QuerySingleOrDefault<Addresses>(sql, new {Id = id});
            }
        }

        public bool Remove(int id) {
            using (var conn = new MySqlConnection(_connStr)) {
                conn.Open();
                var sql = @"
                    DELETE FROM Addresses WHERE Id = @id
                ";
                return conn.Execute(sql, new {id}) == 1;
            }
        }

        public bool Update(Addresses model) {
            using (var conn = new MySqlConnection(_connStr)) {
                conn.Open();
                var sql = $@"
                    UPDATE Addresses SET
                      StreetAddress1 = {nameof(Addresses.StreetAddress1)},
                      StreetAddress2 = {nameof(Addresses.StreetAddress2)},
                      ZipCode = {nameof(Addresses.ZipCode)},
                      City = {nameof(Addresses.City)},
                      State = {nameof(Addresses.State)},
                    WHERE
                      Id = {nameof(Addresses.Id)}
                ";

                return conn.Execute(sql, model) == 1;
            }
        }
    }
}