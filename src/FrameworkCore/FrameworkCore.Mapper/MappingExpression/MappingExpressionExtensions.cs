﻿
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace FrameworkCore.Mapper.MappingExpression
{
    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            var sourceType = typeof(TSource);

            var destinationProperties = typeof(TDestination).GetProperties(flags);

            foreach (var property in destinationProperties)
            {
                var propInfoSrc = sourceType.GetProperties().FirstOrDefault(p => p.Name == property.Name);

                if (propInfoSrc == null)
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }
            return expression;
        }
    }
}
