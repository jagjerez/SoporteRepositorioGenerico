
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoporteRepositorioGenericoJG;
using SoporteRepositorioGenericoPruebas.Unidades_de_Trabajo;
using SoporteRepositorioGenericoPruebas.Unidades_de_Trabajo.Interfaces;
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
            servicios.AddScoped<IPruebaUnitOfWork, pruebaUnitOfWork>();
        }
    }
}
