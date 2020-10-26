using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SoporteRepositorioGenericoJG;
using SoporteRepositorioGenericoPruebas.DB.DAO;
using SoporteRepositorioGenericoPruebas.Dependecias;
using SoporteRepositorioGenericoPruebas.Unidades_de_Trabajo.Interfaces;
using System;
using System.Linq;

namespace SoporteRepositorioGenericoPruebas
{
    public class OperacionesRepositorioGenerico
    {
        IPruebaUnitOfWork pruebaUnidadTrabajo;
        bool esperar = true;
        [SetUp]
        public void Setup()
        {
            IServiceProvider service = DependencyInjector.GetServiceProvider();
            pruebaUnidadTrabajo = service.GetService<IPruebaUnitOfWork>();
            pruebaUnidadTrabajo.NotificacionCambioEntidad += PruebaRepository_NotificacionCambioEntidad;

        }
        /// <summary>
        /// evento que se desencadena al realizar cualquier modificacion sobre una entidad dentro de un repositorio generico
        /// </summary>
        /// <param name="e"></param>
        private void PruebaRepository_NotificacionCambioEntidad(SoporteRepositorioGenericoJG.Eventos.NotificacionCambiosEventArgs e)
        {
            Assert.IsTrue(e.EntidadesCambiadas.Count > 0);
            esperar = false;
        }
        /// <summary>
        /// obtener todos los datos de una entidad
        /// </summary>
        [Test]
        public void ObtenerTodosDatos()
        {
            var prueba =  pruebaUnidadTrabajo.RepositorioGenerico.ObtenerTodo();
            Assert.IsNotNull(prueba);
        }
        /// <summary>
        /// test de guardar una entidad con un vento desencadenante
        /// </summary>
        [Test]
        public void GuardarConEvento()
        {
            esperar = true;
            pruebaUnidadTrabajo.RepositorioGenerico.Anadir(new pruebaDao() { valor = "sexo" });
            pruebaUnidadTrabajo.Save();
            while (esperar) { }
        }


    }
}