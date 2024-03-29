﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FrameworkCore.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrameworkCore.Infrastructure.Extensions
{
    public static class ModelBuilderExtenions
    {
        /// <summary>
        ///     Scan and apply Config/Mapping for Tables/Entities 
        /// </summary>
        /// <param name="builder"> </param>
        /// <param name="assembly"></param>
        public static void AddConfigFromAssembly(this ModelBuilder builder, Assembly assembly)
        {
            // Types that do entity mapping
            var mappingTypes = GetMappingTypes(assembly);

            builder.AddConfigFromMappingTypes(mappingTypes);
        }

        /// <summary>
        ///     Scan and apply Config/Mapping for Tables/Entities (into <see cref="DbContext" />) 
        /// </summary>
        /// <param name="builder"> </param>
        /// <param name="assembly"></param>
        public static void AddConfigFromAssembly<TDbContext>(this ModelBuilder builder, Assembly assembly)
            where TDbContext : class
        {
            // Types that do entity mapping
            var mappingTypes = GetMappingTypes(assembly);

            var dbSetTypes =
                typeof(TDbContext)
                    .GetProperties()
                    .Where(x => x.PropertyType.Name == "DbSet`1")
                    .Select(x => x.PropertyType.GetGenericArguments().First())
                    .ToList();

            // Filter mapping types by TDbContext dbSetTypes
            mappingTypes = mappingTypes.Where(x =>
                dbSetTypes.Any(y => y.FullName == x.BaseType?.GetGenericArguments().First().FullName)).ToList();

            builder.AddConfigFromMappingTypes(mappingTypes);
        }

        private static List<Type> GetMappingTypes(Assembly assembly)
        {
            var mappingTypes = assembly.GetTypes()
                .Where(
                    x => x.GetInterfaces()
                        .Any(y => y.GetTypeInfo().IsGenericType &&
                                  y.GetGenericTypeDefinition() == typeof(ITypeConfiguration<>))
                ).ToList();

            return mappingTypes;
        }

        private static void AddConfigFromMappingTypes(this ModelBuilder builder, IEnumerable<Type> mappingTypes)
        {
            // Get the generic Entity method of the ModelBuilder type
            var entityMethod = typeof(ModelBuilder).GetMethods()
                .Single(x => x.Name == nameof(Entity) &&
                             x.IsGenericMethod &&
                             x.ReturnType.Name == $"{nameof(EntityTypeBuilder)}`1");

            foreach (var mappingType in mappingTypes)
            {
                // Get the type of entity to be mapped
                var genericTypeArg = mappingType.GetInterfaces().Single().GenericTypeArguments.Single();

                // Get the method builder.Entity<TEntity>
                var genericEntityMethod = entityMethod.MakeGenericMethod(genericTypeArg);

                // Invoke builder.Entity<TEntity> to get a builder for the entity to be mapped
                var entityBuilder = genericEntityMethod.Invoke(builder, null);

                // Create the mapping type and do the mapping
                var mapper = Activator.CreateInstance(mappingType);
                mapper.GetType().GetMethod("Configure")?.Invoke(mapper, new[] { entityBuilder });
            }
        }

        /// <summary>
        ///     Set Delete Behavior as Restrict in Relationship for disable cascading delete 
        /// </summary>
        /// <param name="builder"></param>
        public static void DisableCascadingDelete(this ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        /// <summary>
        ///     Remove plural table name 
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Skip Shadow Types
                if (entityType.ClrType == null)
                {
                    continue;
                }

                entityType.Relational().TableName = entityType.ClrType.Name;
            }
        }

        /// <summary>
        ///     Replace table name by new value 
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="oldValue">    </param>
        /// <param name="newValue">    </param>
        public static void ReplaceTableNameConvention(this ModelBuilder modelBuilder, string oldValue, string newValue)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Skip Shadow Types
                if (entityType.ClrType == null)
                {
                    continue;
                }

                entityType.Relational().TableName = entityType.Relational().TableName.Replace(oldValue, newValue);
            }
        }

        /// <summary>
        ///     Replace table name by new value 
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="oldValue">    </param>
        /// <param name="newValue">    </param>
        public static void ReplaceColumnNameConvention(this ModelBuilder modelBuilder, string oldValue, string newValue)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Skip Shadow Types
                if (entityType.ClrType == null)
                {
                    continue;
                }

                foreach (var property in entityType.GetProperties())
                {
                    property.Relational().ColumnName = property.Name.Replace(oldValue, newValue);
                }
            }
        }
    }
}
