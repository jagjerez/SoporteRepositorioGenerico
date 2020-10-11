using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SoporteRepositorioGenericoJG;
using SoporteRepositorioGenericoPruebas.DB.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SoporteRepositorioGenericoPruebas.DB
{
    public class ContextDB : DbContext, IUnidadTrabajo, ISoporteParaTransaccionesContextoDB
    {
        public DbSet<pruebaDao> pruebaDao { get; set; }
        public ContextDB(DbContextOptions options):base(options)
        {

        }
        private IDbContextTransaction _currentTransaction;
        public virtual bool HasActiveTransaction => _currentTransaction != null;

        public virtual IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

        public virtual async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            var result = await base.SaveChangesAsync(cancellationToken);

            return true;
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
                await SaveChangesAsync();
                transaction.Commit();
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


    }
}
