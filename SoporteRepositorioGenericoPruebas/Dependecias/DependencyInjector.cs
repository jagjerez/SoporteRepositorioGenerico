using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using SoporteRepositorioGenericoPruebas.Dependecias.Configuraciones;
using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace SoporteRepositorioGenericoPruebas.Dependecias
{
    internal static class DependencyInjector
    {
        //public static IConfiguration Configuration;
        public static IServiceProvider GetServiceProvider()
        {
            ServiceCollection services = new ServiceCollection();
            ConfiguracionServicios.Configure(services);
            var container = new ContainerBuilder();
            container.Populate(services);
            return new AutofacServiceProvider(container.Build());
        }

        

        
    }
}
