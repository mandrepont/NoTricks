using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;
using NoTricks.Data.Models;

namespace NoTricks.Data.Repositories {
    public interface IAddressRepo : IRepository<Address> {
    }

    public class AddressesRepo : IAddressRepo {
        private readonly string _connStr;

        public AddressesRepo(NoTricksConnectionString connStr) {
            _connStr = connStr.Value;
        }

        public int Insert(Address model) {
            using (var conn = new MySqlConnection(_connStr)) {
                conn.Open();
                var sql = $@"
                    INSERT INTO Addresses (Id, StreetAddress1, StreetAddress2, ZipCode, City, State)
                    VALUES(
                      @{nameof(Address.Id)}, @{nameof(Address.StreetAddress1)}, @{nameof(Address.StreetAddress2)},
                      @{nameof(Address.ZipCode)}, @{nameof(Address.City)}, @{nameof(Address.State)}
                    );
                    SELECT @@IDENTITY;    
                ";
                return conn.Query<int>(sql, model).Single();
            }
        }

        public Address GetById(int id) {
            using (var conn = new MySqlConnection(_connStr)) {
                conn.Open();
                var sql = $@"
                    SELECT
                      Id AS {nameof(Address.Id)},
                      StreetAddress1 AS {nameof(Address.StreetAddress1)},
                      StreetAddress2 AS {nameof(Address.StreetAddress2)},
                      ZipCode AS {nameof(Address.ZipCode)},
                      City AS {nameof(Address.City)},
                      State AS {nameof(Address.State)},
                    FROM Addresses
                    WHERE Id = @Id;
                ";

                return conn.QuerySingleOrDefault<Address>(sql, new {Id = id});
            }
        }

        public IEnumerable<Address> GetAll() {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                    SELECT
                      Id AS {nameof(Address.Id)},
                      StreetAddress1 AS {nameof(Address.StreetAddress1)},
                      StreetAddress2 AS {nameof(Address.StreetAddress2)},
                      ZipCode AS {nameof(Address.ZipCode)},
                      City AS {nameof(Address.City)},
                      State AS {nameof(Address.State)},
                    FROM Addresses                   
                ";

            return conn.Query<Address>(sql);
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

        public bool Update(Address model) {
            using (var conn = new MySqlConnection(_connStr)) {
                conn.Open();
                var sql = $@"
                    UPDATE Addresses SET
                      StreetAddress1 = {nameof(Address.StreetAddress1)},
                      StreetAddress2 = {nameof(Address.StreetAddress2)},
                      ZipCode = {nameof(Address.ZipCode)},
                      City = {nameof(Address.City)},
                      State = {nameof(Address.State)},
                    WHERE
                      Id = {nameof(Address.Id)}
                ";

                return conn.Execute(sql, model) == 1;
            }
        }
    }
}