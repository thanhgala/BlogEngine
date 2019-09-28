using System.Threading.Tasks;
using MediatR;

namespace Blog.Domain.Core.Commands
{
    /// <summary>
    /// Interface IMediatorHandler
    /// </summary>
    public interface ICommandDispatcher
    {
        /// <summary>
        /// Sends the specified command.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task SendCommand<T>(T command) where T : Command;

        Task<TResponse> SendQuery<TResponse> (IRequest<TResponse> request) where TResponse : class;
    }
}
