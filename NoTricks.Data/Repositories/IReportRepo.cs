using System;
using NoTricks.Data.Models.Report;
using System.Collections.Generic;

namespace NoTricks.Data.Repositories {
    public interface IReportRepo {
        IEnumerable<AccountStatusCount> GetAccountStatusCount(bool fillData);
        IEnumerable<AccountCreatedCount> GetAccountCreatedCount(DateTime startDateTime, DateTime endDateTime);
        Counts GetCounts();
    }
}
