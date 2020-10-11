
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoporteRepositorioGenericoJG;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoporteRepositorioGenericoPruebas.Dependecias.Declaraciones
{
    public static class Dominio
    {
        public static void Inyectar(IServiceCollection servicios)
        {
            servicios.AddScoped(typeof(IRepositorioGenerico<>), typeof(RepositorioGenerico<>));
            servicios.AddScoped<IRepositorioGenerico,RepositorioGenerico>();

        }
    }
}
