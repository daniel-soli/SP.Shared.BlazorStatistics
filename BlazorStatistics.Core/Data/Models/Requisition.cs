using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazorStatistics.Core.Data.Models
{
    public class Requisition
    {
        [Key]
        public int Id { get; set; }

        public DateTime RequisitionCreatedTime { get; set; }

        public int LabHerId { get; set; }

        public int SenderHerId { get; set; }

    }

    public class RequisiotionFull
    {
        [Key]
        public int Id { get; set; }
        public DateTime RequisitionCreatedTime { get; set; }

        public int SenderHerId { get; set; }

        public string SenderName { get; set; }
        public int OrganizationId { get; set; }

        public int LabHerId { get; set; }
        public string ServProviderName { get; set; }
        public int ServProviderId { get; set; }
        public string ServiceName { get; set; }
        public string LabName { get; set; }
    }

    public class TopSender
    {
        [Key]
        public int SenderHerId { get; set; }
        public string SenderName { get; set; }
        public string SenderShortName { get; set; }
        public int Antall { get; set; }
    }

    public class TopReceiver
    {
        [Key]
        public int LabHerId { get; set; }
        public string LabName { get; set; }
        public string LabShortName { get; set; }
        public int Antall { get; set; }
    }

    public class ThisWeek
    {
        [Key]
        public int WeekNumber { get; set; }
        public int Antall { get; set; }
    }

    public class ServProvider
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
