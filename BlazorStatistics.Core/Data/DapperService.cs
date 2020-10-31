using BlazorStatistics.Core.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace BlazorStatistics.Core.Data
{
    public class DapperService : IDapperService
    {
        private readonly IConfiguration _config;

        public DapperService(IConfiguration config)
        {
            _config = config;
        }
        public DbConnection GetConnection()
        {
            // MS SQL
            //return new SqlConnection
            //   (_config.GetConnectionString("RequisitionDB"));
            // MY SQL
            return new MySqlConnection
               (_config.GetConnectionString("RequisitionDB"));
        }

        public T Get<T>(string sp, DynamicParameters parms,
         CommandType commandType = CommandType.StoredProcedure)
        {
            // MS SQL
            //using IDbConnection db = new SqlConnection
            //   (_config.GetConnectionString("RequisitionDB"));
            // MY SQL
            using IDbConnection db = new MySqlConnection
               (_config.GetConnectionString("RequisitionDB"));
            return db.Query<T>(sp, parms,
               commandType: commandType).FirstOrDefault();
        }
        public List<T> GetAll<T>(string sp, DynamicParameters parms,
           CommandType commandType = CommandType.StoredProcedure)
        {
            // MS SQL
            //using IDbConnection db = new SqlConnection
            //   (_config.GetConnectionString("RequisitionDB"));
            // MY SQL
            using IDbConnection db = new MySqlConnection
               (_config.GetConnectionString("RequisitionDB"));
            return db.Query<T>(sp, parms,
               commandType: commandType).ToList();
        }
        public int Execute(string sp, DynamicParameters parms,
           CommandType commandType = CommandType.StoredProcedure)
        {
            // MS SQL
            //using IDbConnection db = new SqlConnection
            //   (_config.GetConnectionString("RequisitionDB"));
            // MY SQL
            using IDbConnection db = new MySqlConnection
               (_config.GetConnectionString("RequisitionDB"));
            return db.Execute(sp, parms, commandType: commandType);
        }
        public T Insert<T>(string sp, DynamicParameters parms,
           CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            // MS SQL
            //using IDbConnection db = new SqlConnection
            //   (_config.GetConnectionString("RequisitionDB"));
            // MY SQL
            using IDbConnection db = new MySqlConnection
               (_config.GetConnectionString("RequisitionDB"));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType:
                       commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }
            return result;
        }
        public T Update<T>(string sp, DynamicParameters parms,
           CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            // MS SQL
            //using IDbConnection db = new SqlConnection
            //   (_config.GetConnectionString("RequisitionDB"));
            // MY SQL
            using IDbConnection db = new MySqlConnection
               (_config.GetConnectionString("RequisitionDB"));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType:
                       commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }
            return result;
        }
        public void Dispose()
        {
        }
    }
}
