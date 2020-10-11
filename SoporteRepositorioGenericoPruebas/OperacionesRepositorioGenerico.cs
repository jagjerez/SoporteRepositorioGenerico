using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SoporteRepositorioGenericoJG;
using SoporteRepositorioGenericoPruebas.DB.DAO;
using SoporteRepositorioGenericoPruebas.Dependecias;
using System;
using System.Linq;

namespace SoporteRepositorioGenericoPruebas
{
    public class OperacionesRepositorioGenerico
    {
        IRepositorioGenerico<pruebaDao> pruebaRepository;
        bool esperar = true;
        [SetUp]
        public void Setup()
        {
            IServiceProvider service = DependencyInjector.GetServiceProvider();
            pruebaRepository = service.GetService<IRepositorioGenerico<pruebaDao>>();
            pruebaRepository.NotificacionCambioEntidad += PruebaRepository_NotificacionCambioEntidad;

        }
        /// <summary>
        /// evento que se desencadena al realizar cualquier modificacion sobre una entidad dentro de un repositorio generico
        /// </summary>
        /// <param name="e"></param>
        private void PruebaRepository_NotificacionCambioEntidad(SoporteRepositorioGenericoJG.Eventos.NotificacionCambiosEventArgs e)
        {
            Assert.IsTrue(!(e.tiposCambio == SoporteRepositorioGenericoJG.Enums.TipoCambiosEntidad.none));
            esperar = false;
        }
        /// <summary>
        /// obtener todos los datos de una entidad
        /// </summary>
        [Test]
        public void ObtenerTodosDatos()
        {
            var prueba =  pruebaRepository.ObtenerTodo().ToList();
            Assert.IsNotNull(prueba);
        }
        /// <summary>
        /// test de guardar una entidad con un vento desencadenante
        /// </summary>
        [Test]
        public void GuardarConEvento()
        {
            esperar = true;
            pruebaRepository.Anadir(new pruebaDao() { valor = "sexo" });
            pruebaRepository.Guardar();
            while (esperar) { }
        }


    }
}