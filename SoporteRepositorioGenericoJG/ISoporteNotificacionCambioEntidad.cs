using SoporteRepositorioGenericoJG.Eventos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SoporteRepositorioGenericoJG
{
    public interface ISoporteNotificacionCambioEntidad
    {
        delegate void NotificacionCambioEntidadManejador(NotificacionCambiosEventArgs e);
        event NotificacionCambioEntidadManejador NotificacionCambioEntidad;
    }
}
