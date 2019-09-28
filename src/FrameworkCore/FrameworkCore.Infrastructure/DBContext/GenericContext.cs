using FrameworkCore.Infrastructure.Entity.Audit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FrameworkCore.Infrastructure.DBContext
{
    public abstract class GenericContext : DbContext
    {
        protected GenericContext()
        {
        }

        protected GenericContext(DbContextOptions options) : base(options)
        {
        }

        public override int SaveChanges()
        {
            AddAuditData();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            AddAuditData();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public void AddAuditData()
        {
            var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
            foreach (var item in modified)
            {
                if (item.Entity is IDateTracking changeOrAddedItem)
                {
                    if (item.State == EntityState.Added)
                    {
                        changeOrAddedItem.DateCreated = DateTime.Now;
                    }
                    changeOrAddedItem.DateModified = DateTime.Now;
                }
            }
        }

        private DbCommand CreateCommand(string text, CommandType type = CommandType.Text, params SqlParameter[] parameters)
        {
            var connection = Database.GetDbConnection();

            DbCommand cmd = connection.CreateCommand();

            cmd.CommandText = text;

            cmd.CommandType = type;

            if (parameters?.Any() == true)
            {
                foreach (var parameter in parameters)
                {
                    var p = cmd.CreateParameter();

                    p.DbType = parameter.DbType;

                    p.ParameterName = parameter.ParameterName;

                    p.Value = parameter.Value;

                    cmd.Parameters.Add(p);
                }
            }

            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }

            connection.Open();

            return cmd;
        }

        public void ExecuteCommand(string text, CommandType type = CommandType.Text, params SqlParameter[] parameters)
        {
            var cmd = CreateCommand(text, type, parameters);

            cmd.ExecuteReader();
        }

        //public List<T> ExecuteCommand<T>(string text, CommandType type = CommandType.Text, params SqlParameter[] parameters) where T : class, new()
        //{
        //    var cmd = CreateCommand(text, type, parameters);

        //    using (var reader = cmd.ExecuteReader())
        //    {
        //        var data = reader.QueryTo<T>();
        //        return data;
        //    }
        //}

    }
}
