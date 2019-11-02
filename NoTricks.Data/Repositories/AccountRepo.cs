using System;
using System.Linq;
using NoTricks.Data.Models;
using MySql.Data.MySqlClient;
using Dapper;

namespace NoTricks.Data.Repositories {
    public class AccountRepo : IRepository<Account> {
        private readonly string _connStr;

        public AccountRepo(NoTricksConnectionString connStr) {
            _connStr = connStr.Value;
        }
        
        public int Insert(Account model) {
            using (var conn = new MySqlConnection(_connStr)) {
                conn.Open();
                var sql = $@"
                  INSERT INTO Accounts (Email, PasswordHash, PasswordSalt, Status, CreatedAt, LastLoginAt, LastModifiedAt)
                  VALUES (
                    @{nameof(Account.EMail)}, @{nameof(Account.PasswordHash)}, @{nameof(Account.PasswordSalt)},
                    @{nameof(Account.Status)}, @{nameof(Account.CreatedAt)}, @{nameof(Account.LastLoginAt)}, @{nameof(Account.LastModifiedAt)}
                  );
                  SELECT @@IDENTITY;
                ";
                return conn.Query<int>(sql, model).Single();
            }
        }

        public Account GetById(int id) {
            using (var conn = new MySqlConnection(_connStr)) {
                conn.Open();
                var sql = $@"
                    SELECT 
                      Id AS {nameof(Account.Id)},
                      Email AS {nameof(Account.EMail)},
                      PasswordHash AS {nameof(Account.PasswordHash)},
                      PasswordSalt AS {nameof(Account.PasswordSalt)},
                      Status AS {nameof(Account.Status)},
                      CreatedAt AS {nameof(Account.CreatedAt)},
                      LastLoginAt AS {nameof(Account.LastLoginAt)},
                      LastModifiedAt AS {nameof(Account.LastModifiedAt)}
                    FROM Accounts
                    WHERE Id = @Id;
                ";
                return conn.QuerySingleOrDefault<Account>(sql, new {Id = id});
            }
        }
        
        public Account GetByEmail(string email) {
            using (var conn = new MySqlConnection(_connStr)) {
                conn.Open();
                var sql = $@"
                    SELECT 
                      Id AS {nameof(Account.Id)},
                      Email AS {nameof(Account.EMail)},
                      PasswordHash AS {nameof(Account.PasswordHash)},
                      PasswordSalt AS {nameof(Account.PasswordSalt)},
                      Status AS {nameof(Account.Status)},
                      CreatedAt AS {nameof(Account.CreatedAt)},
                      LastLoginAt AS {nameof(Account.LastLoginAt)},
                      LastModifiedAt AS {nameof(Account.LastModifiedAt)}
                    FROM Accounts
                    WHERE Email = @Email;
                ";
                return conn.QuerySingleOrDefault<Account>(sql, new {Email = email});
            }
        }

        public bool Remove(int Id) {
            using (var conn = new MySqlConnection(_connStr)) {
                conn.Open();
                var sql = @"
                    DELETE FROM Accounts WHERE Id = @Id
                ";
                return conn.Execute(sql, new {Id}) == 1;
            }
        }

        public bool Update(Account model) {
            using (var conn = new MySqlConnection(_connStr)) {
                conn.Open();
                var sql = $@"
                  UPDATE Accounts SET
                    Email = @{nameof(Account.EMail)},
                    PasswordHash = @{nameof(Account.PasswordHash)},
                    PasswordSalt = @{nameof(Account.PasswordSalt)},
                    Status = @{nameof(Account.Status)},
                    CreatedAt = @{nameof(Account.CreatedAt)},
                    LastLoginAt = @{nameof(Account.LastLoginAt)},
                    LastModifiedAt = @{nameof(Account.LastModifiedAt)}
                  WHERE Id = @{nameof(Account.Id)}
                ";
                return conn.Execute(sql, model) == 1;
            }
        }
    }
}