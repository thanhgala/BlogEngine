﻿using System.Threading.Tasks;
using MediatR;

namespace Blog.Domain.Core.Commands
{
    /// <summary>
    /// Class MediatorHandler. This class cannot be inherited.
    /// </summary>
    public sealed class CommandDispatcher : ICommandDispatcher
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandDispatcher"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public CommandDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Sends the specified command.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public Task<TResponse> SendQuery<TResponse>(IRequest<TResponse> request) where TResponse : class
        {
            return _mediator.Send(request);
        }
    }
}