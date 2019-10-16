using Autofac;
using Blog.Domain.Core.Repositories;
using Blog.Domain.Core.Uow;
using Blog.Infrastructure.Data.Context;
using Blog.Infrastructure.Data.Repositories;
using Blog.Infrastructure.Data.Uow;

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

            builder.RegisterType<BlogEngineUow>().As<IBlogEngineUow>().InstancePerLifetimeScope();

            base.Load(builder);
        }

        
    }
}
