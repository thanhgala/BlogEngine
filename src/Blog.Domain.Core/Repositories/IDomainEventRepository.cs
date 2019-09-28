using System.Threading.Tasks;
using Blog.Domain.Core.Events;

namespace Blog.Domain.Core.Repositories
{
    /// <summary>
    /// Interface IDomainEventRepository
    /// </summary>
    public interface IDomainEventRepository
    {
        /// <summary>
        /// Adds the specified domain event.
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of the t domain event.</typeparam>
        /// <param name="domainEvent">The domain event.</param>
        Task Add<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : DomainEvent;

        //IEnumerable<DomainEventRecord> FindAll();
    }
}
