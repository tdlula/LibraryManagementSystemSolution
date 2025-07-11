using LibraryManagementSystem.Common.Entities;
using LibraryManagementSystem.Common.Interfaces;
using System.Collections.Concurrent;

namespace LibraryManagementSystem.Infrastructure.DAL
{
   /// <summary>
    /// In-memory repository implementation for demonstration
    /// In production, this would be replaced with Entity Framework or similar
    /// </summary>
    public class InMemoryBookRepository : IBookRepository
    {
        private readonly ConcurrentDictionary<Guid, Book> _books = new();
        
        public Task<IEnumerable<Book>> GetAllAsync()
        {
            return Task.FromResult(_books.Values.AsEnumerable());
        }
        
        public Task<Book?> GetByIdAsync(Guid id)
        {
            _books.TryGetValue(id, out var book);
            return Task.FromResult(book);
        }
        
        public Task<Book?> GetByISBNAsync(string isbn)
        {
            var book = _books.Values.FirstOrDefault(b => b.ISBN == isbn);
            return Task.FromResult(book);
        }
        
        public Task<IEnumerable<Book>> SearchAsync(string searchTerm)
        {
            var results = _books.Values.Where(b =>
                b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                b.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                b.ISBN.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            );
            return Task.FromResult(results);
        }
        
        public Task<Book> AddAsync(Book book)
        {
            _books.TryAdd(book.Id, book);
            return Task.FromResult(book);
        }
        
        public Task<Book> UpdateAsync(Book book)
        {
            _books.TryUpdate(book.Id, book, _books[book.Id]);
            return Task.FromResult(book);
        }
        
        public Task<bool> DeleteAsync(Guid id)
        {
            return Task.FromResult(_books.TryRemove(id, out _));
        }
        
        public Task<bool> ExistsAsync(string isbn)
        {
            var exists = _books.Values.Any(b => b.ISBN == isbn);
            return Task.FromResult(exists);
        }
    }
    
    /// <summary>
    /// Unit of Work implementation
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IBookRepository _bookRepository;
        private bool _disposed = false;
        
        public UnitOfWork(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }
        
        public IBookRepository Books => _bookRepository;
        
        public Task<int> SaveChangesAsync()
        {
            // In a real implementation, this would save to database
            return Task.FromResult(1);
        }
        
        public Task BeginTransactionAsync()
        {
            // In a real implementation, this would begin database transaction
            return Task.CompletedTask;
        }
        
        public Task CommitTransactionAsync()
        {
            // In a real implementation, this would commit database transaction
            return Task.CompletedTask;
        }
        
        public Task RollbackTransactionAsync()
        {
            // In a real implementation, this would rollback database transaction
            return Task.CompletedTask;
        }
        
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }
    }
}
