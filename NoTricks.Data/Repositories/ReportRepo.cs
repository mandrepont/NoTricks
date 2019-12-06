using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;
using NoTricks.Data.Models;
using NoTricks.Data.Models.Report;

namespace NoTricks.Data.Repositories {
    public class ReportRepo : IReportRepo {
        private readonly string _connStr;

        public ReportRepo(NoTricksConnectionString connStr) {
            _connStr = connStr.Value;
        }

        public IEnumerable<AccountStatusCount> GetAccountStatusCount(bool fillData) {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                SELECT 
                  Status AS {nameof(AccountStatusCount.Status)},
                  Count(*) AS {nameof(AccountStatusCount.Count)}
                FROM Accounts
                GROUP BY Status
            ";
            if (!fillData)
                return conn.Query<AccountStatusCount>(sql);

            var data = conn.Query<AccountStatusCount>(sql).ToList();
            var empty = Enum.GetValues(typeof(AccountStatus))
                .Cast<AccountStatus>()
                .Where(x => !data.Exists(y => y.Status == x) && x != AccountStatus.Unauthenticated)
                .Select(x => new AccountStatusCount(x, 0));
            return data.Concat(empty);
        }

        public IEnumerable<AccountCreatedCount> GetAccountCreatedCount(DateTime startDateTime, DateTime endDateTime) {
            if (startDateTime >= endDateTime) {
                throw new ArgumentOutOfRangeException(nameof(startDateTime), "startDateTime must be less than endDateTime");
            }

            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                SELECT 
                    (CAST(CreatedAt AS Date)) AS {nameof(AccountCreatedCount.CreatedDate)},
                    Count(*) AS {nameof(AccountCreatedCount.Count)}
                FROM Accounts
                WHERE CreatedAt BETWEEN @startDateTime AND @endDateTime
                GROUP BY CAST(CreatedAt AS Date)
            ";
            var data = conn.Query<AccountCreatedCount>(sql, new {startDateTime, endDateTime}).ToList();
            var days = Enumerable.Range(1, endDateTime.Subtract(startDateTime).Days - 1)
                .Select(offset => startDateTime.AddDays(offset))
                .Where(dateTime => !data.Exists(x => x.CreatedDate.Equals(dateTime)))
                .Select(dateTime => new AccountCreatedCount(dateTime, 0));
            return data.Concat(days);
        }

        public Counts GetCounts() {
            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                SELECT 
                  (SELECT SUM(Balance) FROM Suppliers) AS {nameof(Counts.PendingPayoutSum)},
                  (SELECT COUNT(*) FROM Accounts) AS {nameof(Counts.Accounts)},
                  (SELECT COUNT(*) FROM Suppliers) AS {nameof(Counts.Suppliers)},
                  (SELECT SUM(Amount) FROM SupplierPayouts) AS {nameof(Counts.PayoutSum)}
            ";
            return conn.QuerySingle<Counts>(sql);
        }

        public IEnumerable<SupplierPayoutCount> GetSupplierPayoutCounts(DateTime startDateTime, DateTime endDateTime) {
            if (startDateTime >= endDateTime) {
                throw new ArgumentOutOfRangeException(nameof(startDateTime), "startDateTime must be less than endDateTime");
            }

            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                SELECT 
                    (CAST(PayedAt AS Date)) AS {nameof(SupplierPayoutCount.PayedAt)},
                    Count(*) AS {nameof(SupplierPayoutCount.Count)}
                FROM SupplierPayouts
                WHERE PayedAt BETWEEN @startDateTime AND @endDateTime
                GROUP BY CAST(PayedAt AS Date)
            ";
            var data = conn.Query<SupplierPayoutCount>(sql, new {startDateTime, endDateTime}).ToList();
            var days = Enumerable.Range(1, endDateTime.Subtract(startDateTime).Days - 1)
                .Select(offset => startDateTime.AddDays(offset))
                .Where(dateTime => !data.Exists(x => x.PayedAt.Equals(dateTime)))
                .Select(dateTime => new SupplierPayoutCount(dateTime, 0));
            return data.Concat(days);
        }

        public IEnumerable<SupplierPayoutSum> GetSupplierPayoutSums(DateTime startDateTime, DateTime endDateTime) {
            if (startDateTime >= endDateTime) {
                throw new ArgumentOutOfRangeException(nameof(startDateTime), "startDateTime must be less than endDateTime");
            }

            using var conn = new MySqlConnection(_connStr);
            conn.Open();
            var sql = $@"
                SELECT 
                    (CAST(PayedAt AS Date)) AS {nameof(SupplierPayoutSum.PayedAt)},
                    SUM(Amount) AS {nameof(SupplierPayoutSum.Sum)}
                FROM SupplierPayouts
                WHERE PayedAt BETWEEN @startDateTime AND @endDateTime
                GROUP BY CAST(PayedAt AS Date)
            ";
            var data = conn.Query<SupplierPayoutSum>(sql, new {startDateTime, endDateTime}).ToList();
            var days = Enumerable.Range(1, endDateTime.Subtract(startDateTime).Days - 1)
                .Select(offset => startDateTime.AddDays(offset))
                .Where(dateTime => !data.Exists(x => x.PayedAt.Equals(dateTime)))
                .Select(dateTime => new SupplierPayoutSum(dateTime, 0));
            return data.Concat(days);
        }
    }
}
