using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using SoporteRepositorioGenericoJG.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SoporteRepositorioGenericoJG.Extensiones
{

    public static class EntityFrameworkCoreExtensions
    {

        public static IQueryable<object> Set(this DbContext pContexto, Type tipoEntidad)
        {
            return (IQueryable<object>)pContexto.GetType().GetMethod("Set").MakeGenericMethod(tipoEntidad).Invoke(pContexto, null);
        }

        public static string ObtenerNombreEntidadFisica(this DbContext pContexto, Type tipoEntidad)
        {
            string nombreTabla = null; 
            var modelos = pContexto.Model;

            
            var entityTypes = modelos.GetEntityTypes();
            var entityTypeOfT = entityTypes.First(t => t.ClrType == tipoEntidad);


            var tableNameAnnotation = entityTypeOfT.GetAnnotation("Relational:TableName");
            nombreTabla = tableNameAnnotation.Value.ToString();
            return nombreTabla;
        }

        public static IEnumerable<object> EjecutarConsultaSQL(this DbContext pContexto, ConsultaEntidad pConsultaEntidad)
        {
            Type tipoEntidad = Type.GetType("Sgb_Infrastructure.DAO." + pConsultaEntidad.NombreEntidad );
            
            
            var infoMetodo = typeof(EntityFrameworkCoreExtensions).GetMethod("EjecutarConsultaSQLPura", BindingFlags.Static | BindingFlags.NonPublic);
            var infoMetodoGenerico = infoMetodo.MakeGenericMethod(tipoEntidad);
            var resultados = infoMetodoGenerico.Invoke(null, new object[] { pContexto, pConsultaEntidad});
            return (List<Object>)resultados;

        }
        private static IEnumerable<object> EjecutarConsultaSQLPura<TipoEntidad>(this DbContext pContexto, ConsultaEntidad pConsultaEntidad) where TipoEntidad : class
        {
            string sqlFinal = null;
            if (!pConsultaEntidad.SqlCompleta )
            {
                sqlFinal = "SELECT * FROM " + pContexto.ObtenerNombreEntidadFisica (typeof (TipoEntidad) )  + " " + pConsultaEntidad.Sql;
            }
            else
            {
                sqlFinal = pConsultaEntidad.Sql;                
            }

            var resultados = pContexto.Set<TipoEntidad>().FromSqlRaw(sqlFinal, pConsultaEntidad.Parametros)
                       .Select(e => (object)e)
                       .ToList();
            return resultados;
        }


        public static void EjecutarComandoSQL(this DbContext pContexto, ConsultaEntidad pComandoEntidad)
        {
            Type tipoEntidad = Type.GetType("Sgb_Infrastructure.DAO." + pComandoEntidad.NombreEntidad);

            
            var infoMetodo = typeof(EntityFrameworkCoreExtensions).GetMethod("EjecutarComandoSQLPuro", BindingFlags.Static | BindingFlags.NonPublic);
            var infoMetodoGenerico = infoMetodo.MakeGenericMethod(tipoEntidad);
            infoMetodoGenerico.Invoke(null, new object[] { pContexto, pComandoEntidad});


        }
        private static void EjecutarComandoSQLPuro<TipoEntidad>(this DbContext pContexto, ConsultaEntidad pComandoEntidad) where TipoEntidad : class
        {
            string sqlFinal = null;

            sqlFinal = pComandoEntidad.Sql;

            pContexto.Set<TipoEntidad>().FromSqlRaw(sqlFinal, pComandoEntidad.Parametros);
            
        }

        public static List<PropertyInfo> GetDbSetProperties(this DbContext context)
        {
            var dbSetProperties = new List<PropertyInfo>();
            var properties = context.GetType().GetProperties();

            foreach (var property in properties)
            {
                var setType = property.PropertyType;

                var isDbSet = setType.IsGenericType && (typeof(DbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition()));

                if (isDbSet)
                {
                    dbSetProperties.Add(property);
                }
            }

            return dbSetProperties;

        }
        // Actualmente este método sólo se puede ejecutar sobre una BD MySql debido a la clase MySqlParameter
        public static async Task<int> ExecuteNonQueryAsync(this DbContext context, string rawSql, params object[] parameters)
        {
            var conn = context.Database.GetDbConnection();
            //where Codigo =  @p1
            using (var command = conn.CreateCommand())
            {
                command.CommandText = rawSql;
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                await conn.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
        }
        // Actualmente este método sólo se puede ejecutar sobre una BD MySql debido a la clase MySqlParameter
        public static async Task<T> ExecuteScalarAsync<T>(this DbContext context, string rawSql, params object[] parameters)
        {
            var conn = context.Database.GetDbConnection();
            using (var command = conn.CreateCommand())
            {
                command.CommandText = rawSql;
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                await conn.OpenAsync();
                return (T)await command.ExecuteScalarAsync();
            }
        }
    }
}
