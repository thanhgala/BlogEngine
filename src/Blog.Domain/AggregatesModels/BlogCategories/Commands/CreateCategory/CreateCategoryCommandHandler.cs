using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blog.Domain.AggregatesModels.BlogCategories.Events;
using Blog.Domain.AggregatesModels.BlogCategories.Models;
using Blog.Domain.Core.Events;
using Blog.Domain.Core.Notification;
using Blog.Domain.Core.Repositories;
using Blog.Domain.Core.Uow;
using MediatR;

namespace Blog.Domain.AggregatesModels.BlogCategories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : CommandHandler, IRequestHandler<CreateCategoryCommand,bool>
    {
        private readonly IBlogEngineRepository<BlogCategoryEntity, int> _blogCategoryRepository;
        private readonly IEventDispatcher _eventDispatcher;

        public CreateCategoryCommandHandler(IBlogEngineRepository<BlogCategoryEntity, int> blogCategoryRepository,
            INotificationHandler<DomainNotification> notifications, 
            IEventDispatcher eventDispatcher, 
            IBlogEngineUow uow) : base(notifications, eventDispatcher, uow)
        {
            _blogCategoryRepository = blogCategoryRepository;
            _eventDispatcher = eventDispatcher;
        }


        public async Task<bool> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await NotifyValidationErrors(request);
                return false;
            }

            var entity = new BlogCategoryEntity
            {
                Name = request.Name,
                Slug = request.Slug
            };

            await _blogCategoryRepository.AddAsync(entity);

            if (await Commit())
            {
                await _eventDispatcher.RaiseEvent(new AddCagtegoryEvent {Category = entity});
                return true;
            }

            return false;
        }
    }
}
