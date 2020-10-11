using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SoporteRepositorioGenericoJG
{
    public interface ISoporteParaTransaccionesContextoDB
    {
        IDbContextTransaction GetCurrentTransaction();
        bool HasActiveTransaction { get; }
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel level);
        Task CommitTransactionAsync(IDbContextTransaction transaction);

        void RollbackTransaction();
    }
}
