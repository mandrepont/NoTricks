using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ChartJSCore.Models;
using System.Collections.Generic;
using System.Linq;
using ChartJSCore.Helpers;
using NoTricks.Data.Models;
using NoTricks.Data.Models.Report;
using NoTricks.Data.Repositories;

namespace NoTrick.Web.Pages.Admin {
    public class IndexModel : PageModel {
        private readonly ILogger<IndexModel> _logger;
        private readonly IReportRepo _reportRepo;
        public Chart AccountStatusChart { get; set; }
        public Chart AccountCreatedCountChart { get; set; }
        public Chart SupplierPayoutCountChart { get; set; }
        public Chart SupplierPayoutSumChart  { get; set; }
        public Counts Counts { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IReportRepo reportRepo) {
            _logger = logger;
            _reportRepo = reportRepo;
        }

        public void OnGet() {
            AccountStatusChart = GetAccountStatusChart();
            AccountCreatedCountChart = GetAccountCreatedChart();
            SupplierPayoutSumChart = GetSupplierPayoutSumChart();
            SupplierPayoutCountChart = GetSupplierPayoutCountChart();
            Counts = _reportRepo.GetCounts();
        }

        private Chart GetAccountCreatedChart() {
            _logger.LogTrace("Generating chart for account created over the past 7 days.");
            var accountCreatedData = _reportRepo.GetAccountCreatedCount(DateTime.Today.AddDays(-8), DateTime.Today.AddDays(1)).OrderBy(x => x.CreatedDate).ToList();
            return new Chart {
                Type = Enums.ChartType.Line,
                Data = new Data {
                    Labels = accountCreatedData.Select(x => x.CreatedDate.ToShortDateString()).ToList(),
                    Datasets = new List<Dataset> {
                        new LineDataset {
                            Label = "# Of Accounts Created",
                            Data = accountCreatedData.Select(x => (double) x.Count).ToList(),
                            Fill = "false",
                            LineTension = 0.1,
                            BackgroundColor = ChartColor.FromRgba(75, 192, 192, 0.4),
                            BorderColor = ChartColor.FromRgba(75,192,192,1),
                            BorderCapStyle = "butt",
                            BorderDashOffset = 0.0,
                            BorderJoinStyle = "miter",
                            PointBorderColor = new List<ChartColor>() { ChartColor.FromRgba(75,192,192,1) },
                            PointBackgroundColor = new List<ChartColor>() { ChartColor.FromHexString("#fff") },
                            PointBorderWidth = new List<int> { 1 },
                            PointHoverRadius = new List<int> { 5 },
                            PointHoverBackgroundColor = new List<ChartColor>() { ChartColor.FromRgba(75,192,192,1) },
                            PointHoverBorderColor = new List<ChartColor>() { ChartColor.FromRgba(220,220,220,1) },
                            PointHoverBorderWidth = new List<int> { 2 },
                            PointRadius = new List<int> { 1 },
                            PointHitRadius = new List<int> { 10 },
                        }
                    }
                }
            };
        }
        
        private Chart GetSupplierPayoutSumChart() {
            _logger.LogTrace("Generating chart for total supplier payout over the past 30 days.");
            var supplierPayoutSumData = _reportRepo.GetSupplierPayoutSums(DateTime.Today.AddMonths(-1).AddDays(-1), DateTime.Today.AddDays(1)).OrderBy(x => x.PayedAt).ToList();
            return new Chart {
                Type = Enums.ChartType.Line,
                Data = new Data {
                    Labels = supplierPayoutSumData.Select(x => x.PayedAt.ToShortDateString()).ToList(),
                    Datasets = new List<Dataset> {
                        new LineDataset {
                            Label = "$ Total Payouts",
                            Data = supplierPayoutSumData.Select(x => (double) x.Sum).ToList(),
                            Fill = "false",
                            LineTension = 0.1,
                            BackgroundColor = ChartColor.FromRgba(40, 167, 69, 0.4),
                            BorderColor = ChartColor.FromRgba(40,167,69,1),
                            BorderCapStyle = "butt",
                            BorderDashOffset = 0.0,
                            BorderJoinStyle = "miter",
                            PointBorderColor = new List<ChartColor>() { ChartColor.FromRgba(40,167,69,1) },
                            PointBackgroundColor = new List<ChartColor>() { ChartColor.FromHexString("#fff") },
                            PointBorderWidth = new List<int> { 1 },
                            PointHoverRadius = new List<int> { 5 },
                            PointHoverBackgroundColor = new List<ChartColor>() { ChartColor.FromRgba(40,167,69,1) },
                            PointHoverBorderColor = new List<ChartColor>() { ChartColor.FromRgba(40,220,220,1) },
                            PointHoverBorderWidth = new List<int> { 2 },
                            PointRadius = new List<int> { 1 },
                            PointHitRadius = new List<int> { 10 },
                        }
                    }
                }
            };
        }
        
        private Chart GetSupplierPayoutCountChart() {
            _logger.LogTrace("Generating chart for total number of supplier payout over the past 30 days.");
            var payoutCountData = _reportRepo.GetSupplierPayoutCounts(DateTime.Today.AddMonths(-1).AddDays(-1), DateTime.Today.AddDays(1)).OrderBy(x => x.PayedAt).ToList();
            return new Chart {
                Type = Enums.ChartType.Bar,
                Data = new Data {
                    Labels = payoutCountData.Select(x => x.PayedAt.ToShortDateString()).ToList(),
                    Datasets = new List<Dataset> {
                        new BarDataset {
                            Label = "Total Number Of Payouts",
                            Data = payoutCountData.Select(x => (double) x.Count).ToList(),
                            BackgroundColor = new List<ChartColor>{ ChartColor.FromHexString("#007bff") },
                            HoverBackgroundColor = new List<ChartColor>{ ChartColor.FromHexString("#007bff") },
                        }
                    }
                },
            };
        }

        private Chart GetAccountStatusChart() {
            _logger.LogTrace("Generating chart for account in each status.");
            var colorMapping = new Dictionary<AccountStatus, ChartColor> {
                {AccountStatus.Banned, ChartColor.FromHexString("#dc3545") },
                {AccountStatus.Disabled, ChartColor.FromHexString("#6c757d") },
                {AccountStatus.Ok, ChartColor.FromHexString("#28a745") },
                {AccountStatus.LockedOut, ChartColor.FromHexString("#ffc107") },
                {AccountStatus.PendingVerification, ChartColor.FromHexString("#17a2b8") }
            };
            var accountStatusData = _reportRepo.GetAccountStatusCount(true).ToList();
            var colors = accountStatusData.Select(x => colorMapping[x.Status]).ToList();

            return new Chart {
                Type = Enums.ChartType.Pie,
                Data = new Data {
                    Labels = accountStatusData.Select(x => Enum.GetName(typeof(AccountStatus), x.Status)).ToList(),
                    Datasets = new List<Dataset> {
                        new PieDataset {
                            Label = "Account Statuses (With Empty)",
                            BackgroundColor = colors,
                            HoverBackgroundColor = colors,
                            Data = accountStatusData.Select(x => (double) x.Count).ToList()
                        }
                    }
                }
            };
        }
    }
}
