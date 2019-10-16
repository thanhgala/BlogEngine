using Microsoft.AspNetCore.Identity;
using System.Linq;
using Blog.Domain.Core.Notification;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Blog.Domain.Core.Events;
using FrameworkCore.Web.ApiResponseWrapper.Extensions.Wrappers;

namespace Blog.WebApi.Controllers
{
    public abstract class ApiBaseController : ControllerBase
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly IEventDispatcher _eventDispatcher;


        protected ApiBaseController(INotificationHandler<DomainNotification> notifications, IEventDispatcher eventDispatcher)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _eventDispatcher = eventDispatcher;
        }
        protected bool IsValidOperation()
        {
            return !_notifications.HasNotifications();
        }

        public IActionResult ResponseApi(ApiResponse result = null)
        {
            if (IsValidOperation())
            {
                return Ok(result ?? new ApiResponse());
            };

            return BadRequest(new ApiException(_notifications.Notifications.Select(n => n.Value)));
        }

        protected void NotifyModelStateErrors()
        {
            var erros = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros)
            {
                var erroMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(string.Empty, erroMsg);
            }
        }

        protected void NotifyError(string code, string message)
        {
            _eventDispatcher.RaiseEvent(new DomainNotification(code, message));
        }

        protected void AddIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                NotifyError(result.ToString(), error.Description);
            }
        }
    }
}
