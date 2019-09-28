using System.Threading.Tasks;
using Blog.Domain.Core.Events;
using Blog.Domain.Core.Repositories;
using Blog.Infrastructure.Data.Context;

namespace Blog.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Class DomainEventRepository.
    /// </summary>
    public class DomainEventRepository : IDomainEventRepository
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly EventStoreContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEventRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public DomainEventRepository(EventStoreContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds the specified domain event.
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of the t domain event.</typeparam>
        /// <param name="domainEvent">The domain event.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task Add<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : DomainEvent
        {
            await _context.DomainEventRecords.AddAsync(new DomainEventRecord()
            {
                Created = domainEvent.Created,
                Type = domainEvent.Type,
                Content = domainEvent.Content,
                CorrelationId = domainEvent.CorrelationId
            });

            await _context.SaveChangesAsync();
        }
    }
}
