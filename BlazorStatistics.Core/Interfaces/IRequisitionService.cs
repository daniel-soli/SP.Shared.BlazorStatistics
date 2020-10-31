using BlazorStatistics.Core.Data.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorStatistics.Core.Interfaces
{
    public interface IRequisitionService
    {
        Task<int> Count(string search);
        Task<Requisition> GetById(int Id);
        Task<List<Requisition>> GetByDate(string date);
        Task<List<Requisition>> ListAll(int skip, int take,
            string orderBy, string direction, string search);
        Task<List<Requisition>> ListAll();
        Task<List<RequisiotionFull>> ListAllWithInfo();
        Task<List<TopSender>> GetTopSender();
    }
}
