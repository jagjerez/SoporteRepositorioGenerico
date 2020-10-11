using SoporteRepositorioGenericoJG.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SoporteRepositorioGenericoJG
{
    public interface IRepositorioGenerico<TEntidad>: ISoporteNotificacionCambioEntidad where TEntidad : class
    {

        IEnumerable<TEntidad> ObtenerTodo();

        TEntidad Obtener(object pId);

        void Anadir(TEntidad pElementoEntidad);

        void Modificar(TEntidad pElementoEntidad);

        void Borrar(TEntidad pElementoEntidad);

        void Borrar(object pId);

        void Guardar();


        IEnumerable<TEntidad> ObtenerConFiltroMedianteExpresion(
                    Expression<Func<TEntidad, bool>> pExpresionFiltro = null,
                    Func<IQueryable<TEntidad>, IOrderedQueryable<TEntidad>> pExpresionOrden = null,
                    string pPropiedadesIncluidas = "");

        IQueryable<TEntidad> ObtenerConFiltroMedianteExpresion(
                    Expression<Func<TEntidad, bool>> pExpresionFiltro = default(Expression<Func<TEntidad, bool>>));

        IQueryable<TEntidad> ObtenerConFiltroMedianteSQL(string pConsulta,
                    params object[] pParametros);
    }
    public interface IRepositorioGenerico: ISoporteNotificacionCambioEntidad
    {
        IEnumerable<object> ObtenerTodo();

        object Obtener(object pId);

        void Anadir(object pElementoEntidad);

        void Modificar(object pElementoEntidad);

        void Borrar(object pElementoEntidad);

        void BorrarPorClave(object pId);
        void EjecutarComandoSQL(ConsultaEntidad pConsultaEntidad);

        void Guardar();

        IEnumerable<object> ObtenerPorFiltroMedianteSQL(ConsultaEntidad pConsultaEntidad);
    }
}
