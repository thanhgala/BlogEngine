using System;
using System.ComponentModel.DataAnnotations;

namespace FrameworkCore.Infrastructure.Entity
{
    public abstract class DomainEntity<TId>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public virtual TId Id { get; set; }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        /// <value>
        /// The row version.
        /// </value>
        [Timestamp]
        public virtual byte[] RowVersion { get; set; }

        //public virtual EntityState EntityState { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as DomainEntity<TId>);
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public virtual bool Equals(DomainEntity<TId> other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (IsTransient(this) || IsTransient(other) || !Equals(Id, other.Id)) return false;
            var otherType = other.GetUnproxiedType();
            var thisType = GetUnproxiedType();
            return thisType.IsAssignableFrom(otherType) || otherType.IsAssignableFrom(thisType);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => Equals(Id, default(TId)) ? base.GetHashCode() : Id.GetHashCode();

        /// <summary>
        /// Determines whether the specified object is transient.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        private static bool IsTransient(DomainEntity<TId> obj)
        {
            return obj != null &&
            Equals(obj.Id, default(TId));
        }

        /// <summary>
        /// Gets the type of the unproxied.
        /// </summary>
        /// <returns></returns>
        private Type GetUnproxiedType()
        {
            return GetType();
        }
    }
}
