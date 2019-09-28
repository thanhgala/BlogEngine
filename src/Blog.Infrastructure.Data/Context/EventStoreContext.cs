using System.IO;
using Blog.Domain.Core.Events;
using FrameworkCore.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blog.Infrastructure.Data.Context
{
    /// <summary>
    /// Class EventStoreContext.
    /// </summary>
    public class EventStoreContext : GenericContext
    {
        /// <summary>
        /// Gets or sets the domain event records.
        /// </summary>
        /// <value>The domain event records.</value>
        public DbSet<DomainEventRecord> DomainEventRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // get the configuration from the app settings
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            // define the database to use
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
    }
}
