using BlazorStatistics.Core.Data.Models;
using BlazorStatistics.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BlazorStatisticsWebGUI.Pages
{
    public partial class Charts2
    {
        [Inject]
        protected IRequisitionService requisitionService { get; set; }

        protected List<RequisiotionFull> requisitions { get; set; }
        protected IEnumerable<DataItem> reqChartItems { get; set; }
        protected IEnumerable<DataItemDate> reqDChartItems { get; set; }
        protected IEnumerable<ServProvider> servProviders { get; set; }
        protected int dateType { get; set; } = 0;
        public int? ItemId = null;
        IEnumerable<int> checkBoxValues = new int[] {  };

        public class DataItem
        {
            public string Label { get; set; }
            public int Count { get; set; }
        }
        public class DataItemDate
        {
            public DateTime Label { get; set; }
            public int Count { get; set; }
        }
        public int PeriodId
        {
            set
            {
                ItemId = (value == 0 ? (int?)null : value);
                Load();
            }
            get
            {
                return (ItemId == null ? 0 : ItemId.Value);
            }
        }
        public List<(int Id, string Name)> selectOptions = new List<(int Id, string Name)>
            {
                (0, "Dager"),
                (1, "Uker"),
                (2, "Måneder"),
                (3, "År"),
            };
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                requisitions = await requisitionService.ListAllWithInfo();
                reqDChartItems = LoadDaily();
                servProviders = GetServProviders();
                
                StateHasChanged();
            }
        }

        void Load()
        {
            Console.WriteLine($"{checkBoxValues}");
            int[] values = checkBoxValues.ToArray();
            switch (PeriodId)
            {
                case 1:
                    reqChartItems = LoadWeekly(values);
                    break;
                case 2:
                    reqChartItems = LoadMonthly(values);
                    break;
                case 0:
                    reqDChartItems = LoadDaily(values);
                    break;
                default:
                    reqChartItems = LoadYearly(values);
                    break;
            }
        }

        void Change(IEnumerable<int> value, string name)
        {
            var str = string.Join(", ", value);
            Console.WriteLine($"{str}");
            Console.WriteLine($"{PeriodId}");
            int[] values = value.ToArray();
            switch (PeriodId)
            {
                case 1:
                    reqChartItems = LoadWeekly(values);
                    break;
                case 2:
                    reqChartItems = LoadMonthly(values);
                    break;
                case 0:
                    reqDChartItems = LoadDaily(values);
                    break;
                default:
                    reqChartItems = LoadYearly(values);
                    break;
            }
        }
        
        private IEnumerable<DataItemDate> LoadDaily(int[] servs = null)
        {
            dateType = 0;
            if (servs == null || servs.Length == 0)
            {
                var reqByDay = requisitions
                .GroupBy(s => s.RequisitionCreatedTime.Date)
                .Select(g => new DataItemDate
                {
                    Label = g.Key,
                    Count = g.Select(l => l.Id)
                .Distinct()
                .Count()
                });
                return reqByDay;
            }
            else
            {
                var reqByDay = requisitions
                .Where(w => servs.Contains(w.ServProviderId))
                .GroupBy(s => s.RequisitionCreatedTime.Date)
                .Select(g => new DataItemDate
                {
                    Label = g.Key,
                    Count = g.Select(l => l.Id)
                .Distinct()
                .Count()
                });
                return reqByDay;
            }
        }

        private IEnumerable<DataItem> LoadWeekly(int[] servs = null)
        {
            dateType = 1;
            if (servs == null || servs.Length == 0)
            {
                var reqByWeek = requisitions
                .GroupBy(i => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(i.RequisitionCreatedTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                .Select(g => new DataItem
                {
                    Label = g.Key.ToString(),
                    Count = g.Select(l => l.Id)
                .Distinct()
                .Count()
                });

                return reqByWeek;
            }
            else
            {
                var reqByWeek = requisitions
                    .Where(w => servs.Contains(w.ServProviderId))
                    .GroupBy(i => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(i.RequisitionCreatedTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                    .Select(g => new DataItem
                    {
                        Label = g.Key.ToString(),
                        Count = g.Select(l => l.Id)
                    .Distinct()
                    .Count()
                    });

                return reqByWeek;
            }
            
        }
        private IEnumerable<DataItem> LoadMonthly(int[] servs = null)
        {
            dateType = 1;
            if (servs == null || servs.Length == 0)
            {
                var reqByMonth = requisitions
                    .GroupBy(s => new { s.RequisitionCreatedTime.Year, s.RequisitionCreatedTime.Month })
                    .Select(g => new DataItem { Label = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month), Count = g.Select(l => l.Id).Distinct().Count() });

                return reqByMonth;
            }
            else
            {
                var reqByMonth = requisitions
                    .Where(w => servs.Contains(w.ServProviderId))
                    .GroupBy(s => new { s.RequisitionCreatedTime.Year, s.RequisitionCreatedTime.Month })
                    .Select(g => new DataItem { Label = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month), Count = g.Select(l => l.Id).Distinct().Count() });

                return reqByMonth;
            }
            
        }
        private IEnumerable<DataItem> LoadYearly(int[] servs = null)
        {
            dateType = 1;
            if (servs == null || servs.Length == 0)
            {
                List<DataItem> reqs = new List<DataItem>();
                
                reqs = requisitions
                    .GroupBy(s => s.RequisitionCreatedTime.Year)
                    .Select(g => new DataItem { Label = g.Key.ToString(), Count = g.Count() }).ToList();
                reqs.Insert(0, new DataItem
                {
                    Label = "2019",
                    Count = 0
                });
                reqs.ToArray();

                return reqs;
            }
            else
            {
                List<DataItem> reqs = new List<DataItem>();
                
                reqs = requisitions
                    .Where(w => servs.Contains(w.ServProviderId))
                    .GroupBy(s => s.RequisitionCreatedTime.Year)
                    .Select(g => new DataItem { Label = g.Key.ToString(), Count = g.Count() }).ToList();
                reqs.Insert(0, new DataItem
                {
                    Label = "2019",
                    Count = 0
                });
                reqs.ToArray();
                return reqs;
            }
        }

        private IEnumerable<ServProvider> GetServProviders()
        {
            var servprov = requisitions
                .GroupBy(g => new { g.ServProviderId, g.ServProviderName })
                .Select(s => new ServProvider
                {
                    Id = s.Key.ServProviderId,
                    Name = s.Key.ServProviderName
                })
                .OrderBy(y => y.Name);
            
            return servprov;
        }
    }
}
