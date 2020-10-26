using SoporteRepositorioGenericoJG;
using SoporteRepositorioGenericoPruebas.DB.DAO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoporteRepositorioGenericoPruebas.Unidades_de_Trabajo.Interfaces
{
    public interface IPruebaUnitOfWork:ISoporteNotificacionCambioEntidad,ISoporteParaTransaccionesContextoDB
    {
        IRepositorioGenerico<pruebaDao> RepositorioGenerico { get; }
        IRepositorioGenerico RepositorioGenericoSinTipo { get; }
        int Save();
    }
}
