using System;
using System.Collections.Generic;
using System.Text;

namespace SoporteRepositorioGenericoJG.Entidades
{

    public class ConsultaEntidad
    {
        // Nombre de la entidad corresponde al nombre completo (FullName) del tipo de al entidad en el modelo
        public string NombreEntidad { get; set; }
        // Si se quieren todos los datos no se necesita indicar la propiedad Sql
        // Por simplicidad inicalmente la Sql tendrá los parámetros incluidos en la parte WHERE
        // Más adelante se cogerán para evitar inyección de SQL
        public string Sql { get; set; }
        public object[] Parametros { get; set; }
        // En caso de llamar a procedimientos almacenados se deberá indicar que use la use la Sql completa que se le pase: SqlCompleta con valor true
        public bool SqlCompleta { get; set; } 

    }
}
