using Microsoft.EntityFrameworkCore;
using SoporteRepositorioGenericoJG.Entidades;
using SoporteRepositorioGenericoJG.Eventos;
using SoporteRepositorioGenericoJG.Extensiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using static SoporteRepositorioGenericoJG.ISoporteNotificacionCambioEntidad;

namespace SoporteRepositorioGenericoJG
{
    public class RepositorioGenerico<TEntidad> : IRepositorioGenerico<TEntidad> where TEntidad : class
    {
        protected DbSet<TEntidad> entidad = null;
        protected DbContext contexto = null;

        public RepositorioGenerico(DbSet<TEntidad> pEntidad,DbContext pContexto)
        {
            entidad = pEntidad;
            contexto = pContexto;
        }

        public virtual IEnumerable<TEntidad> ObtenerTodo()
        {
            IEnumerable<TEntidad> datos = null;
            try
            {
                datos = entidad.ToList();
                return datos;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public virtual TEntidad Obtener(object pId)
        {
            TEntidad dato = null;

            try
            {
                dato = entidad.Find(pId);
                return dato;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public virtual void Anadir(TEntidad pElementoEntidad)
        {
            try
            {
                entidad.Add(pElementoEntidad);
                
            }
            catch (Exception)
            {

                throw;
            }

        }

        public virtual void Modificar(TEntidad pElementoEntidad)
        {
            try
            {
                entidad.Attach(pElementoEntidad);
                contexto.Entry(pElementoEntidad).State = EntityState.Modified;
                
            }
            catch (Exception)
            {

                throw;
            }

        }

        public virtual void Borrar(TEntidad pElementoEntidad)
        {
            try
            {
                TEntidad elemento = entidad.Find(pElementoEntidad);
                if (contexto.Entry(pElementoEntidad).State == EntityState.Detached)
                {
                    entidad.Attach(elemento);
                }
                entidad.Remove(elemento);
               
            }
            catch (Exception)
            {

                throw;
            }

        }

        public virtual void Borrar(object pId)
        {
            try
            {
                TEntidad elemento = entidad.Find(pId);

                if (contexto.Entry(elemento).State == EntityState.Detached)
                {
                    entidad.Attach(elemento);
                }
                entidad.Remove(elemento);
                
            }
            catch (Exception)
            {

                throw;
            }

        }

        public virtual IEnumerable<TEntidad> ObtenerConFiltroMedianteExpresion(Expression<Func<TEntidad, bool>> pExpresionFiltro = null, Func<IQueryable<TEntidad>, IOrderedQueryable<TEntidad>> pExpresionOrden = null, string pPropiedadesIncluidas = "")
        {
            IQueryable<TEntidad> consulta = entidad;

            if (pExpresionFiltro != null)
            {
                consulta = consulta.Where(pExpresionFiltro);
            }

            if (pPropiedadesIncluidas != null)
            {
                foreach (var includeProperty in pPropiedadesIncluidas.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    consulta = consulta.Include(includeProperty);
                }
            }


            if (pExpresionOrden != null)
            {
                return pExpresionOrden(consulta).ToList();
            }
            else
            {
                return consulta.ToList();
            }
        }

        public virtual IQueryable<TEntidad> ObtenerConFiltroMedianteExpresion(Expression<Func<TEntidad, bool>> pExpresionFiltro = default(Expression<Func<TEntidad, bool>>))
        {
            if (pExpresionFiltro == null)
                throw new ArgumentNullException();
            return entidad.Where(pExpresionFiltro);
        }

        public virtual IQueryable<TEntidad> ObtenerConFiltroMedianteSQL(string pConsulta, params object[] pParametros)
        {
            return entidad.FromSqlRaw(pConsulta, pParametros);
        }


    }

    public class RepositorioGenerico : IRepositorioGenerico
    {
        protected DbContext contexto;
        protected IQueryable<object> entidad = null;
        protected string nombreEntidad;
        protected string nombreTipoEntidad;
        protected Type tipoEntidad;

        public RepositorioGenerico(DbContext pContexto, string pNombreEntidad)
        {
            try
            {
                this.contexto = pContexto;

                Type tipoDbSet = typeof(DbSet<>);
                Type tipoEntidad = Type.GetType(pNombreEntidad);
                Type tipoDbSetGenerico = tipoDbSet.MakeGenericType(tipoEntidad);          
                nombreEntidad = pNombreEntidad;
                nombreTipoEntidad = tipoEntidad.FullName;
                entidad = contexto.Set(tipoEntidad);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public IEnumerable<object> ObtenerTodo()
        {
            try
            {
                var datos = entidad.AsEnumerable();

                return datos;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public object Obtener(object pId)
        {
            object dato = null;

            try
            {
                dato = contexto.Find(tipoEntidad, pId);
                return dato;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Anadir(object pElementoEntidad)
        {
            try
            {
                contexto.Add(pElementoEntidad);
               
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Modificar(object pElementoEntidad)
        {
            try
            {
                contexto.Entry(pElementoEntidad).State = EntityState.Modified;

            
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Borrar(object pElementoEntidad)
        {
            try
            {
                object elemento = contexto.Find(tipoEntidad, pElementoEntidad);
                if (contexto.Entry(pElementoEntidad).State == EntityState.Detached)
                {
                    contexto.Attach(elemento);
                }
                contexto.Remove(elemento);
              
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void BorrarPorClave(object pId)
        {
            try
            {
                object elemento = contexto.Find(tipoEntidad, pId);
                if (contexto.Entry(elemento).State == EntityState.Detached)
                {
                    contexto.Attach(elemento);
                }
                contexto.Remove(elemento);
                
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IEnumerable<object> ObtenerPorFiltroMedianteSQL(ConsultaEntidad pConsultaEntidad)
        {

            return contexto.EjecutarConsultaSQL(pConsultaEntidad).ToList();
        }


        public void EjecutarComandoSQL(ConsultaEntidad pConsultaEntidad)
        {

            int resultado = contexto.ExecuteNonQueryAsync(pConsultaEntidad.Sql, pConsultaEntidad.Parametros).Result;
        }


    }
}
