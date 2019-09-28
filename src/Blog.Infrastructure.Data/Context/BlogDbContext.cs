using System.Reflection;
using Blog.Domain.AggregatesModels.BlogCategories.Models;
using Blog.Domain.AggregatesModels.Blogs.Models;
using FrameworkCore.Infrastructure.DBContext;
using FrameworkCore.Infrastructure.Extensions;
using FrameworkCore.Utils.ConfigUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blog.Infrastructure.Data.Context
{
    public sealed class BlogDbContext : GenericContext
    {
        public readonly int CommandTimeoutInSecond = 20 * 60;
        public BlogDbContext()
        {
            Database.SetCommandTimeout(CommandTimeoutInSecond);
        }
        public BlogDbContext(DbContextOptions options) : base(options)
        {
            Database.SetCommandTimeout(CommandTimeoutInSecond);
        }

        public DbSet<BlogEntity> Blogs { set; get; }

        public DbSet<BlogCategoryEntity> BlogCategories { set; get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("connectionconfig.json", false, true).Build();

                var connectionString = config.GetValueByEnv<string>("ConnectionStrings");

                optionsBuilder.UseSqlServer(connectionString, sqlServerOptionsAction =>
                {
                    sqlServerOptionsAction.MigrationsAssembly(typeof(BlogDbContext).GetTypeInfo().Assembly.GetName().Name);

                    sqlServerOptionsAction.MigrationsHistoryTable("Migration");
                });
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.AddConfigFromAssembly<BlogDbContext>(typeof(BlogDbContext).GetTypeInfo().Assembly);

            builder.DisableCascadingDelete();

            builder.RemovePluralizingTableNameConvention();

            builder.ReplaceTableNameConvention("Entity", string.Empty);
        }
    }
}
