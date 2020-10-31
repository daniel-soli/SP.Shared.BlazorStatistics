using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using BlazorStatistics.Core.Data;

namespace BlazorStatistics.Core.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBlazorDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("ReqStatistics");


            if (section.GetValue<string>("DbProvider") == "SqlServer")
            {
                AppSettings.DbOptions = options => options.UseSqlServer(section.GetValue<string>("ConnString"));
            }
            else if (section.GetValue<string>("DbProvider") == "MySql")
            {
                AppSettings.DbOptions = options => options.UseMySql(section.GetValue<string>("ConnString"));
            }
            else
            {
                AppSettings.DbOptions = options => options.UseSqlite(section.GetValue<string>("ConnString"));
            }

            services.AddDbContext<AppDbContext>(AppSettings.DbOptions, ServiceLifetime.Scoped);



            return services;
        }
    }
}
