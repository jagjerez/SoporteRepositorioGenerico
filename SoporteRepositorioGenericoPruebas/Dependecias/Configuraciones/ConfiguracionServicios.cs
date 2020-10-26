using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using SoporteRepositorioGenericoPruebas.Dependecias.Declaraciones;

namespace SoporteRepositorioGenericoPruebas.Dependecias.Configuraciones
{
    public static class ConfiguracionServicios
    {
        public static void Configure(IServiceCollection servicios)
        {
            servicios.AddSingleton<IConfiguration>(CreacionConfiguracion.Inyectar());
            //servicios.AddAutoMapper(typeof(DependencyInjector));
            //BDConsiguracion.Inyectar(servicios);
            Dominio.Inyectar(servicios);
            LoggerModuloInyeccion.Inyectar(servicios);
        }
    }
}
