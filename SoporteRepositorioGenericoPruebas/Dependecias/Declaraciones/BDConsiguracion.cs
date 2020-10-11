
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoporteRepositorioGenericoPruebas.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoporteRepositorioGenericoPruebas.Dependecias.Declaraciones
{
    public static class BDConsiguracion
    {
        
        public static void Inyectar(IServiceCollection servicios)
        {
            var provider = servicios.BuildServiceProvider();
            IConfiguration configuration = provider.GetService<IConfiguration>();
            switch (configuration["MotorDB"])
            {
                case "Mysql":
                    if (configuration["ASPNETCORE_ENVIRONMENT"] == "Production")
                    {
                        servicios.AddDbContext<DbContext, ContextDB>(options =>
                            options.UseMySQL(configuration["MysqlConnectionPro"]));
                    }
                    else
                    {
                        servicios.AddDbContext<DbContext, ContextDB>(options =>
                            options.UseMySQL(configuration["MysqlConnectionDev"]));
                    }
                    break;

                case "Sql":
                    if (configuration["ASPNETCORE_ENVIRONMENT"] == "Production")
                    {
                        servicios.AddDbContext<DbContext, ContextDB>(options =>
                            options.UseSqlServer(configuration["SqlConnectionPro"]));
                    }
                    else
                    {
                        servicios.AddDbContext<DbContext, ContextDB>(options =>
                            options.UseSqlServer(configuration["SqlConnectionDev"]));
                    }
                    break;

                case "Mongo":
                    //if (configuration["ASPNETCORE_ENVIRONMENT"] == "Production")
                    //{
                    //    servicios.AddDbContext<DbContext, ContextDB>(options =>
                    //        options.UseMySQL(configuration["SqlConnectionPro"]));
                    //}
                    //else
                    //{
                    //    servicios.AddDbContext<DbContext, ContextDB>(options =>
                    //        options.UseMySQL(configuration["SqlConnectionDev"]));
                    //}
                    break;
            }
            
        }
    }
}
