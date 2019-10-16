using System.Threading.Tasks;
using Blog.Domain.Core.Commands;
using Blog.Domain.Core.Events;
using Blog.Domain.Core.Notification;
using Blog.Domain.Core.Uow;
using MediatR;

namespace Blog.Domain.AggregatesModels.BlogCategories.Commands
{
    public class CommandHandler
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IBlogEngineUow _uow;

        public CommandHandler(INotificationHandler<DomainNotification> notifications, IEventDispatcher eventDispatcher, IBlogEngineUow uow)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _eventDispatcher = eventDispatcher;
            _uow = uow;
        }

        protected async Task NotifyValidationErrors(Command message)
        {
            foreach (var error in message.ValidationResult.Errors)
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(message.MessageType, error.ErrorMessage));
            }
        }

        public async Task<bool> Commit()
        {
            if (_notifications.HasNotifications()) return false;
            if (await _uow.CommitChangesAsync()) return true;
            await _eventDispatcher.RaiseEvent(new DomainNotification("Commit",
                "We had a problem during saving your data."));
            return false;

        }
    }
}
