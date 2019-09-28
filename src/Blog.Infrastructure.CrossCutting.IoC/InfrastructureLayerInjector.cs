using Autofac;
using Blog.Domain.Core.Repositories;
using Blog.Infrastructure.Data.Context;
using Blog.Infrastructure.Data.Repositories;

namespace Blog.Infrastructure.CrossCutting.IoC
{
    public class InfrastructureLayerInjector : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<UnitOfWork<,>>().As<IUnitOfWork<,>>().InstancePerLifetimeScope();
            builder.RegisterType<BlogDbContext>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(BlogEngineRepository<,>))
                .As(typeof(IBlogEngineRepository<,>)).InstancePerLifetimeScope();

            base.Load(builder);
        }

        
    }
}
