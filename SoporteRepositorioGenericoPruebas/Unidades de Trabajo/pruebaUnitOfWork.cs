using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoporteRepositorioGenericoJG;
using SoporteRepositorioGenericoJG.Entidades;
using SoporteRepositorioGenericoJG.Eventos;
using SoporteRepositorioGenericoPruebas.DB.DAO;
using SoporteRepositorioGenericoPruebas.DB.DAO.Mapeadores;
using SoporteRepositorioGenericoPruebas.Unidades_de_Trabajo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoporteRepositorioGenericoPruebas.Unidades_de_Trabajo
{
    public class pruebaUnitOfWork : DbContext, IPruebaUnitOfWork
    {
        public DbSet<pruebaDao> Prueba { get; set; }
        private IDbContextTransaction _currentTransaction;
        private IRepositorioGenerico<pruebaDao> repositorioGenerico;
        private IRepositorioGenerico repositorioGenericoSinTipo;
        internal IConfiguration configuracion;
        
        public IRepositorioGenerico<pruebaDao> RepositorioGenerico { 
            get 
            {
                return this.repositorioGenerico;            
            } 
        }
        public IRepositorioGenerico RepositorioGenericoSinTipo
        {
            get
            {
                return this.repositorioGenericoSinTipo;
            }
        }

        public bool HasActiveTransaction => _currentTransaction != null;

        public event ISoporteNotificacionCambioEntidad.NotificacionCambioEntidadManejador NotificacionCambioEntidad;
        public pruebaUnitOfWork(IServiceProvider pServiceProvider) : base()
        {
            this.configuracion = pServiceProvider.GetService<IConfiguration>();
            this.Initialize();
        }
        public pruebaUnitOfWork(DbContextOptions pOpciones) : base(pOpciones)
        {
            this.Initialize();
        }

        public void Initialize()
        {
            this.repositorioGenerico = new RepositorioGenerico<pruebaDao>(Prueba, (DbContext)base.MemberwiseClone());
            this.repositorioGenericoSinTipo = new RepositorioGenerico((DbContext)base.MemberwiseClone(),typeof(pruebaDao).AssemblyQualifiedName);
        }
        private void OnNotificacionCambioEntidad(NotificacionCambiosEventArgs e)
        {
            ISoporteNotificacionCambioEntidad.NotificacionCambioEntidadManejador notificacionCambioEntidadManejador = NotificacionCambioEntidad;
            if (notificacionCambioEntidadManejador != null)
            {
                notificacionCambioEntidadManejador?.Invoke(e);
            }
        }

        public virtual async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public virtual async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel level)
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(level);

            return _currentTransaction;
        }

        public virtual async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                List<EntidadCambiada> entidades = new List<EntidadCambiada>();
                foreach(EntityEntry datos in this.ChangeTracker.Entries())
                {
                    if(datos.State != EntityState.Detached && datos.State != EntityState.Unchanged)
                    {
                        entidades.Add(new EntidadCambiada() { nombreEntidad = datos.Entity.GetType().Name,tipoCambio = datos.State });
                    }
                }
                await SaveChangesAsync();
                transaction.Commit();
                if (entidades.Count > 0)
                {
                    OnNotificacionCambioEntidad(new NotificacionCambiosEventArgs() { EntidadesCambiadas = entidades });
                }
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public virtual IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

        public virtual void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public int Save()
        {
            int saveChangesResult;

            try
            {
                List<EntidadCambiada> entidades = new List<EntidadCambiada>();
                foreach (EntityEntry datos in this.ChangeTracker.Entries())
                {
                    if (datos.State != EntityState.Detached && datos.State != EntityState.Unchanged)
                    {
                        entidades.Add(new EntidadCambiada() { nombreEntidad = datos.Entity.GetType().Name, tipoCambio = datos.State });
                    }
                }
                saveChangesResult = this.SaveChanges();
                if (entidades.Count > 0)
                {
                    OnNotificacionCambioEntidad(new NotificacionCambiosEventArgs() { EntidadesCambiadas = entidades });
                }
            }
            catch (Exception)
            {
                saveChangesResult = 0;
            }

            return saveChangesResult;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration<pruebaDao>(new pruebaDaoMap());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (configuracion["MotorDB"])
            {
                case "Mysql":
                    if (configuracion["ASPNETCORE_ENVIRONMENT"] == "Production")
                    {
                        optionsBuilder.UseMySQL(configuracion["MysqlConnectionPro"]);
                    }
                    else
                    {
                        optionsBuilder.UseMySQL(configuracion["MysqlConnectionDev"]);
                    }
                    break;

                case "Sql":
                    if (configuracion["ASPNETCORE_ENVIRONMENT"] == "Production")
                    {
                        optionsBuilder.UseSqlServer(configuracion["SqlConnectionPro"]);
                    }
                    else
                    {
                        optionsBuilder.UseSqlServer(configuracion["SqlConnectionDev"]);
                    }
                    break;

                case "Mongo":
                    //if (configuration["ASPNETCORE_ENVIRONMENT"] == "Production")
                    //{
                    //    servicios.AddDbContext<DbContext, ContextDB>(options =>
                    //        options.UseMySQL(configuration["SqlConnectionPro"]));
                    //}
                    //else
                    //{
                    //    servicios.AddDbContext<DbContext, ContextDB>(options =>
                    //        options.UseMySQL(configuration["SqlConnectionDev"]));
                    //}
                    break;
            }
            
        }
    }
}
