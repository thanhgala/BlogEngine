using System.Reflection;
using Autofac;
using Blog.Domain.AggregatesModels.BlogCategories.Queries.GetAllBlogCategories;
using Blog.Domain.Core.Commands;
using Blog.Domain.Core.Events;
using Blog.Domain.Core.Notification;
using MediatR;
using Module = Autofac.Module;

namespace Blog.Infrastructure.CrossCutting.IoC
{
    public class DomainLayerInjector : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            //    .AsImplementedInterfaces();

            // Domain Bus (Mediator)
            builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>().InstancePerLifetimeScope();
            builder.RegisterType<EventDispatcher>().As<IEventDispatcher>().InstancePerLifetimeScope();

            // Domain - Events

            //builder.RegisterType<INotificationHandler<DomainNotification>>().As<DomainNotificationHandler>().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(DomainNotificationHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));

            // Domain - Commands
            //builder.RegisterType<IRequestHandler<GetAllBlogsQuery, List<BlogDto>>>()
            //    .As<GetAllBlogsQueryHandler>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(GetAllBlogCategoriesQueryHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            base.Load(builder);

        }
    }
}
