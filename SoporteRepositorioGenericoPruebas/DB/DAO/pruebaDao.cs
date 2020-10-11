using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SoporteRepositorioGenericoPruebas.DB.DAO
{
    [Table("pruebas")]
    public class pruebaDao
    {
        [Key]
        public int id { get; set; }
        public string valor { get; set; }
    }
}
