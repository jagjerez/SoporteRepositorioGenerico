using Microsoft.EntityFrameworkCore;
using SoporteRepositorioGenericoJG.Entidades;
using SoporteRepositorioGenericoJG.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoporteRepositorioGenericoJG.Eventos
{
    public class NotificacionCambiosEventArgs:EventArgs
    {
        public NotificacionCambiosEventArgs()
        {
            EntidadesCambiadas = new List<EntidadCambiada>();
        }
        public List<EntidadCambiada> EntidadesCambiadas { get; set; }
    }
    
}
