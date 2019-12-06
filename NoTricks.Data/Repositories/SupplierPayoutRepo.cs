using System.Collections.Generic;
using MySql.Data.MySqlClient;
﻿using System.Linq;
using Dapper;
using NoTricks.Data.Models;

namespace NoTricks.Data.Repositories {
    public interface ISupplierPayoutRepo : IRepository<SupplierPayout> {}

    public class SupplierPayoutRepo : ISupplierPayoutRepo {
        private readonly string _connStr;

        public SupplierPayoutRepo(NoTricksConnectionString connStr) {
            _connStr = connStr.Value;
        }

        public int Insert(SupplierPayout model) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
              START TRANSACTION;
              INSERT INTO SupplierPayouts(Amount, PayedAt, SupplierId, StaffId)
              VALUES(
                @{nameof(SupplierPayout.Amount)},@{nameof(SupplierPayout.PayedAt)},
                @{nameof(SupplierPayout.SupplierId)},@{nameof(SupplierPayout.StaffId)}
              );
              SELECT @@IDENTITY;
              COMMIT;
            ";
            return conn.Query<int>(sql, model).Single();
        }

        public SupplierPayout GetById(int id) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
              SELECT 
                Id AS {nameof(SupplierPayout.Id)},
                Amount AS {nameof(SupplierPayout.Amount)},
                PayedAt AS {nameof(SupplierPayout.PayedAt)},
                SupplierId AS {nameof(SupplierPayout.SupplierId)},
                StaffId AS {nameof(SupplierPayout.StaffId)}
              FROM SupplierPayouts
              WHERE Id = @Id;
            ";
            return conn.QuerySingleOrDefault<SupplierPayout>(sql, new {Id = id});
        }

        public IEnumerable<SupplierPayout> GetAll() {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
              SELECT 
                Id AS {nameof(SupplierPayout.Id)},
                Amount AS {nameof(SupplierPayout.Amount)},
                PayedAt AS {nameof(SupplierPayout.PayedAt)},
                SupplierId AS {nameof(SupplierPayout.SupplierId)},
                StaffId AS {nameof(SupplierPayout.StaffId)}
              FROM SupplierPayouts
            ";
            return conn.Query<SupplierPayout>(sql);
        }

        public bool Remove(int id) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = @"
              DELETE FROM SupplierPayouts WHERE Id = @id
            ";
            return conn.Execute(sql, new {id}) == 1;
        }

        public bool Update(SupplierPayout model) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
              UPDATE SupplierPayouts SET
                Amount = @{nameof(SupplierPayout.Amount)},
                PayedAt = @{nameof(SupplierPayout.PayedAt)},
                SupplierId = @{nameof(SupplierPayout.SupplierId)},
                StaffId = @{nameof(SupplierPayout.StaffId)}
              WHERE Id = @{nameof(SupplierPayout.Id)};
            ";
            return conn.Execute(sql, model) == 1;
        }
    }
}