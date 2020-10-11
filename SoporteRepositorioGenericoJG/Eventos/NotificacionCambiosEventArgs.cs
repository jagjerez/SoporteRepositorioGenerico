using SoporteRepositorioGenericoJG.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoporteRepositorioGenericoJG.Eventos
{
    public class NotificacionCambiosEventArgs:EventArgs
    {
        public TipoCambiosEntidad tiposCambio { get; set; }
    }
    
}
