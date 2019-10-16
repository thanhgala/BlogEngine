using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Threading.Tasks;

namespace FrameworkCore.Infrastructure.DAL
{
    public class UnitOfWork<TContext> : IUnitOfWork
        where TContext : DbContext
    {
        #region Fields
        private readonly TContext _context;
        public IDbContextTransaction CurrentTransaction { get; set; }
        #endregion

        #region Constructor

        public UnitOfWork(TContext context)
        {
            _context = context;
        }

        #endregion

        public IDbContextTransaction BeginTransaction(IsolationLevel level)
        {
            if (CurrentTransaction != null)
            {
                throw new InvalidOperationException("There is already a running transaction. Cannot create a new one.");
            }
            //open connection
            if (_context.Database.GetDbConnection().State == ConnectionState.Closed)
            {
                _context.Database.OpenConnection();
            }
            var dbTransaction = _context.Database.BeginTransaction(level);
            CurrentTransaction = dbTransaction;
            return dbTransaction;
        }

        public virtual void CommitChanges()
        {
            CommitTransaction();
            _context.SaveChanges();
        }

        public virtual async Task<bool> CommitChangesAsync()
        {
            //CommitTransaction();
            return await _context.SaveChangesAsync() > 0;
        }

        private void CommitTransaction()
        {
            if (CurrentTransaction == null)
            {
                throw new InvalidOperationException("The CurrentTransaction must not be null when committing the current transaction.");
            }
            // commit transaction
            CurrentTransaction.Commit();

            // dispose transaction
            CurrentTransaction.Dispose();
            CurrentTransaction = null;

            // close connection
            _context.Database.CloseConnection();
        }

        public virtual void RollbackTransaction()
        {
            if (CurrentTransaction == null)
            {
                throw new InvalidOperationException("The CurrentTransaction must not be null when committing the current transaction.");
            }

            // rollback transaction
            CurrentTransaction.Rollback();

            // dispose transaction
            CurrentTransaction.Dispose();
            CurrentTransaction = null;

            // close connection
            _context.Database.CloseConnection();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
