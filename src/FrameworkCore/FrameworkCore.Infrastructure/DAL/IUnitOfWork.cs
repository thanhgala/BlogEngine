using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FrameworkCore.Infrastructure.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        IDbContextTransaction BeginTransaction(IsolationLevel level);

        /// <summary>
        /// Commits the changes.
        /// </summary>
        void CommitChanges();

        /// <summary>
        /// Commits the changes.
        /// </summary>
        Task<bool> CommitChangesAsync();

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        void RollbackTransaction();
    }
}
