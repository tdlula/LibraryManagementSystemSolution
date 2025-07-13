using System;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Common.Interfaces
{
    /// <summary>
    /// Unit of Work pattern for transaction management and repository access.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the book repository.
        /// </summary>
        IBookRepository Books { get; }
        /// <summary>
        /// Saves changes to the underlying data store.
        /// </summary>
        Task<int> SaveChangesAsync();
        /// <summary>
        /// Begins a transaction.
        /// </summary>
        Task BeginTransactionAsync();
        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        Task CommitTransactionAsync();
        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        Task RollbackTransactionAsync();
    }
}
