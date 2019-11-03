using System.Collections.Generic;
using MySql.Data.MySqlClient;
﻿using System.Linq;
using Dapper;
using NoTricks.Data.Models;

namespace NoTricks.Data.Repositories {
    public interface IProfileRepo : IRepository<Profile> {
    }

    public class ProfileRepo : IProfileRepo {
        private readonly string _connStr;

        public ProfileRepo(NoTricksConnectionString connStr) {
            _connStr = connStr.Value;
        }

        public int Insert(Profile model) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
              START TRANSACTION;
              INSERT INTO Profile(
              VALUES
                @{nameof(Profile.FirstName)},@{nameof(Profile.LastName)},@{nameof(Profile.PreferredName)},
                @{nameof(Profile.Phone)},@{nameof(Profile.Birthday)},@{nameof(Profile.AddressId)},
                @{nameof(Profile.AccountId)}
              );
              SELECT @@IDENTITY;
              COMMIT;
            ";
            return conn.Query<int>(sql, model).Single();
        }

        public Profile GetById(int id) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
              SELECT 
                Id AS @{nameof(Profile.Id)},
                FirstName AS @{nameof(Profile.FirstName)},
                LastName AS @{nameof(Profile.LastName)},
                PreferredName AS @{nameof(Profile.PreferredName)},
                Phone AS @{nameof(Profile.Phone)},
                Birthday AS @{nameof(Profile.Birthday)},
                AddressId AS @{nameof(Profile.AddressId)},
                AccountId AS @{nameof(Profile.AccountId)},
              FROM Profile
              WHERE Id = @Id
            ";
            return conn.QuerySingleOrDefault<Profile>(sql, new {Id = id});
        }

        public IEnumerable<Profile> GetAll() {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
              SELECT 
                Id AS @{nameof(Profile.Id)},
                FirstName AS @{nameof(Profile.FirstName)},
                LastName AS @{nameof(Profile.LastName)},
                PreferredName AS @{nameof(Profile.PreferredName)},
                Phone AS @{nameof(Profile.Phone)},
                Birthday AS @{nameof(Profile.Birthday)},
                AddressId AS @{nameof(Profile.AddressId)},
                AccountId AS @{nameof(Profile.AccountId)},
              FROM Profile
            ";
            return conn.Query<Profile>(sql);
        }

        public bool Remove(int id) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = @"
              DELETE FROM Profiles WHERE Id = @id
            ";
            return conn.Execute(sql, new {id}) == 1;
        }

        public bool Update(Profile model) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
              UPDATE Profile SET
                FirstName = @{nameof(Profile.FirstName)},
                LastName = @{nameof(Profile.LastName)},
                PreferredName = @{nameof(Profile.PreferredName)},
                Phone = @{nameof(Profile.Phone)},
                Birthday = @{nameof(Profile.Birthday)},
                AddressId = @{nameof(Profile.AddressId)},
                AccountId = @{nameof(Profile.AccountId)}
              WHERE Id = @{nameof(Profile.Id)}
            ";
            return conn.Execute(sql, model) == 1;
        }
    }
}