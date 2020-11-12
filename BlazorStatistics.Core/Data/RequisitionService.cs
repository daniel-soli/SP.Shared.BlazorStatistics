using BlazorStatistics.Core.Data.Models;
using BlazorStatistics.Core.Interfaces;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace BlazorStatistics.Core.Data
{
    public class RequisitionService : IRequisitionService
    {
        private readonly IDapperService _dapperService;

        public RequisitionService(IDapperService dapperService)
        {
            this._dapperService = dapperService;
        }

        // MYSQL
        public Task<Requisition> GetById(int id)
        {
            var requisition = Task.FromResult
               (_dapperService.Get<Requisition>
               ($"select * from Statistics where Id = {id}", null,
               commandType: CommandType.Text));
            return requisition;
        }

        public Task<List<Requisition>> GetByDate(string date)
        {
            var requisition = Task.FromResult
               (_dapperService.GetAll<Requisition>
               ($"select * from Statistics where RequisitionCreatedTime LIKE '{date}%'", null,
               commandType: CommandType.Text));
            return requisition;
        }

        public Task<int> Count(string search)
        {
            var totrequisition = Task.FromResult(_dapperService.Get<int>
               ($"select COUNT(*) from Statistics WHERE id like '%{search}%'", null,
   
               commandType: CommandType.Text));
            return totrequisition;
        }

        public Task<List<Requisition>> ListAll(int skip, int take,
         string orderBy, string direction = "DESC", string search = "")
        {
            var requisition = Task.FromResult
               (_dapperService.GetAll<Requisition>
               ($"SELECT * FROM Statistics WHERE Id like '%{search}%' ORDER BY { orderBy} { direction} OFFSET { skip} ROWS FETCH NEXT { take } ROWS ONLY; ", null, commandType: CommandType.Text));
         return requisition;
        }

        public Task<List<Requisition>> ListAll()
        {
            var requisition = Task.FromResult
               (_dapperService.GetAll<Requisition>
               ($"SELECT Id, CAST(RequisitionCreatedTime AS DATE) as RequisitionCreatedTime, LabHerId, SenderHerId FROM Statistics", null, commandType: CommandType.Text));
            return requisition;
        }

        public Task<List<RequisiotionFull>> ListAllWithInfo()
        {
            var requisition = Task.FromResult
                (_dapperService.GetAll<RequisiotionFull>
                ($"SELECT " +
                    $"Statistics.Id," +
                    $"Statistics.RequisitionCreatedTime, " +
                    $"Statistics.SenderHerId, " +
                    $"CONCAT_WS('', Organizations.Name, ', ', OrganizationServices.Name) as SenderName, " +
                    //$"Organizations.Name" +
                    $"Organizations.Id AS OrganizationId, " +
                    $"Statistics.LabHerId, " +
                    $"CONCAT_WS('', ServProviders.Name, ', ', ServProviders.ServiceName) as LabName, " +
                    $"ServProviders.ServiceName, " +
                    $"ServProviders.Id AS ServProviderId, " + 
                    $"ServProviders.Name AS ServProviderName " +
                    $"FROM Statistics " +
                    $"INNER JOIN OrganizationServices ON Statistics.SenderHerId = OrganizationServices.HerId " +
                    $"INNER JOIN ServProviders ON Statistics.LabHerId = ServProviders.ServiceHerId " +
                    $"INNER JOIN Organizations ON OrganizationServices.OrganizationId = Organizations.Id", null, commandType: CommandType.Text));
            return requisition;
        }
        
        public Task<List<TopSender>> GetTopSender()
        {
            var requisition = Task.FromResult(
                _dapperService.GetAll<TopSender>
                ($"SELECT Statistics.SenderHerId, " +
                $"CONCAT_WS('', Organizations.Name, ', ', OrganizationServices.Name) as SenderName, " +
                $"Count(*) AS Antall " +
                $"FROM `Statistics` " +
                $"INNER JOIN OrganizationServices ON Statistics.SenderHerId = OrganizationServices.HerId " +
                $"INNER JOIN Organizations ON OrganizationServices.OrganizationId = Organizations.Id " +
                $"GROUP BY Statistics.SenderHerId, SenderName " +
                $"ORDER BY Antall DESC " +
                $"LIMIT 6", null, commandType: CommandType.Text)
                );
            return requisition;
        }

        public Task<List<TopReceiver>> GetTopReceivers()
        {
            var requisition = Task.FromResult(
                _dapperService.GetAll<TopReceiver>
                ($"SELECT Statistics.LabHerId, " +
                $"CONCAT_WS('', ServProviders.Name, ', ', ServProviders.ServiceName) as LabName, " +
                $"Count(*) AS Antall " +
                $"FROM `Statistics` " +
                $"INNER JOIN ServProviders ON ServProviders.ServiceHerId = Statistics.LabHerId " +
                $"GROUP BY Statistics.LabHerId, LabName " +
                $"ORDER BY Antall DESC " +
                $"LIMIT 6", null, commandType: CommandType.Text)
                );
            return requisition;
        }
        // MS SQL
        //public Task<Requisition> GetById(int id)
        //{
        //    var requisition = Task.FromResult
        //       (_dapperService.Get<Requisition>
        //       ($"select * from [Statistics] where Id = {id}", null,
        //       commandType: CommandType.Text));
        //    return requisition;
        //}

        //public Task<List<Requisition>> GetByDate(string date)
        //{
        //    var requisition = Task.FromResult
        //       (_dapperService.GetAll<Requisition>
        //       ($"select * from [Statistics] where [RequisitionCreatedTime] LIKE '{date}%'", null,
        //       commandType: CommandType.Text));
        //    return requisition;
        //}

        //public Task<int> Count(string search)
        //{
        //    var totrequisition = Task.FromResult(_dapperService.Get<int>
        //       ($"select COUNT(*) from [Statistics] WHERE [Id] like '%{search}%'", null,

        //       commandType: CommandType.Text));
        //    return totrequisition;
        //}

        //public Task<List<Requisition>> ListAll(int skip, int take,
        // string orderBy, string direction = "DESC", string search = "")
        //{
        //    var requisition = Task.FromResult
        //       (_dapperService.GetAll<Requisition>
        //       ($"SELECT * FROM [Statistics] WHERE [Id] like '%{search}%' ORDER BY { orderBy} { direction} OFFSET { skip} ROWS FETCH NEXT { take } ROWS ONLY; ", null, commandType: CommandType.Text));
        //    return requisition;
        //}

        //public Task<List<Requisition>> ListAll()
        //{
        //    var requisition = Task.FromResult
        //       (_dapperService.GetAll<Requisition>
        //       ($"SELECT [Id], CAST([RequisitionCreatedTime] AS DATE) as [RequisitionCreatedTime], [LabHerId], [SenderHerId] FROM [Statistics]", null, commandType: CommandType.Text));
        //    return requisition;
        //}
    }
}
