using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SoporteRepositorioGenericoPruebas.Dependecias.Declaraciones
{
    public static class CreacionConfiguracion
    {
        public static IConfiguration Inyectar()
        {
            return new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile(
                                     path: "appsettings.json",
                                     optional: false,
                                     reloadOnChange: true)
                               .Build();
        }
    }
}
