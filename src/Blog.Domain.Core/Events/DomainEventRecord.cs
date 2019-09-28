
using System;
using Blog.Domain.Core.Models;

namespace Blog.Domain.Core.Events
{
    /// <summary>
    /// Class DomainEventRecord.
    /// </summary>
    public class DomainEventRecord : BaseEntity<Guid>
    {
        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        /// <value>The correlation identifier.</value>
        public Guid CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; }
    }
}
