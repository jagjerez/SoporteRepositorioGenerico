using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoporteRepositorioGenericoJG.Entidades
{
    public class EntidadCambiada
    {
        public string nombreEntidad { get; set; }
        public EntityState tipoCambio { get; set; }
    }
}
