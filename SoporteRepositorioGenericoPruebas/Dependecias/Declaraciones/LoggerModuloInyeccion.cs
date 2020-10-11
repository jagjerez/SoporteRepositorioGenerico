using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoporteRepositorioGenericoPruebas.Dependecias.Declaraciones
{
    public static class LoggerModuloInyeccion
    {
        public static void Inyectar(IServiceCollection servicios)
        {
            servicios.AddSingleton<ILoggerFactory, LoggerFactory>();
            servicios.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        }
    }
}
