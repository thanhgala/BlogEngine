using System.Reflection;
using Autofac;
using Blog.Application.Services;
using Module = Autofac.Module;

namespace Blog.Infrastructure.CrossCutting.IoC
{
    public class ApplicationLayerInjector : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(BlogCategoryService).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
