using BlazorStatistics.Core.Data.Models;
using BlazorStatistics.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BlazorStatisticsWebGUI.Pages
{
    public partial class Dashboard
    {
        [Inject]
        protected IRequisitionService requisitionService { get; set; }

        protected List<RequisiotionFull> requisitions { get; set; }
        protected List<DataItem> reqlwChart { get; set; }
        protected List<DataItem> reqldChart { get; set; }
        protected IEnumerable<DataItem> reqldChartTest { get; set; }
        protected IEnumerable<DataItem> reqlwChartTest { get; set; }
        int reqsLastWeekCount { get; set; }
        int reqsLastDayCount { get; set; }
        bool fetchingData { get; set; }
        string error = null;
        // INTERNAL CLASSES
        public class DataItem
        {
            public DateTime Label { get; set; }
            public int Count { get; set; }
        }

        //protected override async Task OnInitializedAsync()
        //{
            
        //}

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Console.WriteLine("Henter rekvisisjoner");
                requisitions = await requisitionService.ListAllWithInfo();
                Console.WriteLine("Ferdig med å hente rekvisisjoner...");
                //requisitions = await requisitionService.ListAllWithInfo();
                fetchingData = true;
                try
                {
                    //Console.WriteLine("Starter Load");
                    //await Load();
                    //Console.WriteLine("Starter Load24");
                    //await Load24();
                    Console.WriteLine("Starter LoadWTest");
                    reqlwChartTest = LoadWTest();
                    Console.WriteLine("Starter LoadTest");
                    reqldChartTest = Loadtest();
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
                fetchingData = false;
                StateHasChanged();
            }
        }

        public async Task Load()
        {
            // LAST WEEK
            var reqsLastWeek = requisitions.Where(w => w.RequisitionCreatedTime >= DateTime.Now.Date.AddDays(-6));
            // Count last weeks No reqs
            reqsLastWeekCount = reqsLastWeek.Count();
            // Group last weeks reqs by date for chart
            reqlwChart = reqsLastWeek.GroupBy(g => g.RequisitionCreatedTime.Date)
                .OrderBy(o => o.Key.Date)
                .Select(s => new DataItem{ Count = s.Count(), Label = s.Key}).ToList();
            
            reqlwChart.ToArray();
        }

        public async Task Load24()
        {
            //requisitions = await requisitionService.ListAllWithInfo();
            // LAST 24H
            DateTime now = DateTime.Now;
            var reqLastDay = requisitions.Where(w => w.RequisitionCreatedTime.AddDays(1) >= now);
            // Count last 24 hours No reqs
            reqsLastDayCount = reqLastDay.Count();
            //Group last 24H by hour for chart
            reqldChart = reqLastDay
                .GroupBy(g => new { Date = g.RequisitionCreatedTime.Date, Hour = g.RequisitionCreatedTime.Hour })
                .OrderBy(o => o.Key.Date).ThenBy(o => o.Key.Hour)
                .Select(s => new DataItem { Count = s.Count(), Label = s.Key.Date.AddHours(s.Key.Hour) }).ToList();
            reqldChart.ToArray();
        }

        private IEnumerable<DataItem> Loadtest()
        {
            DateTime now = DateTime.Now;
            var reqLastDay = requisitions.Where(w => w.RequisitionCreatedTime.AddDays(1) >= now);
            // Count last 24 hours No reqs
            //reqsLastDayCount = reqLastDay.Count();
            //Group last 24H by hour for chart
            var reqldChart1 = reqLastDay
                .GroupBy(g => new { Date = g.RequisitionCreatedTime.Date, Hour = g.RequisitionCreatedTime.Hour })
                .OrderBy(o => o.Key.Date).ThenBy(o => o.Key.Hour)
                .Select(s => new DataItem { Count = s.Count(), Label = s.Key.Date.AddHours(s.Key.Hour) }).ToList();
            return reqldChart1;
        }

        private IEnumerable<DataItem> LoadWTest()
        {
            // LAST WEEK
            var reqsLastWeek = requisitions.Where(w => w.RequisitionCreatedTime >= DateTime.Now.Date.AddDays(-6));
            // Count last weeks No reqs
            reqsLastWeekCount = reqsLastWeek.Count();
            // Group last weeks reqs by date for chart
            var reqlwChart1 = reqsLastWeek.GroupBy(g => g.RequisitionCreatedTime.Date)
                .OrderBy(o => o.Key.Date)
                .Select(s => new DataItem { Count = s.Count(), Label = s.Key }).ToList();

            return reqlwChart1;
        }
    }
}
