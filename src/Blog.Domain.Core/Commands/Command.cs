using System;
using FluentValidation.Results;
using MediatR;
using Newtonsoft.Json;

namespace Blog.Domain.Core.Commands
{
    /// <summary>
    /// Class Command.
    /// </summary>
    public abstract class Command : Message
    {
        /// <summary>
        /// Gets or sets the validation result.
        /// </summary>
        /// <value>The validation result.</value>
        public ValidationResult ValidationResult { get; set; }

        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Returns true if ... is valid.
        /// </summary>
        /// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
        public abstract bool IsValid();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        protected Command()
        {
            Timestamp = DateTime.Now;
            MessageType = GetType().Name;
        }
    }

    public abstract class Message : IRequest<bool>
    {
        [JsonProperty]
        public string MessageType { get; set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}
