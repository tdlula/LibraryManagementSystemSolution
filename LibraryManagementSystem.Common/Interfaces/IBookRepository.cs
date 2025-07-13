using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagementSystem.Common.Entities;


namespace LibraryManagementSystem.Common.Interfaces
{
    /// <summary>
    /// Repository pattern interface for book operations.
    /// </summary>
    public interface IBookRepository
    {
        /// <summary>
        /// Gets all books in the repository.
        /// </summary>
        Task<IEnumerable<Book>> GetAllAsync();
        /// <summary>
        /// Gets a book by its unique ID.
        /// </summary>
        Task<Book?> GetByIdAsync(System.Guid id);
        /// <summary>
        /// Gets a book by its ISBN.
        /// </summary>
        Task<Book?> GetByISBNAsync(string isbn);
        /// <summary>
        /// Searches for books by a search term (title, author, or ISBN).
        /// </summary>
        Task<IEnumerable<Book>> SearchAsync(string searchTerm);
        /// <summary>
        /// Adds a new book to the repository.
        /// </summary>
        Task<Book> AddAsync(Book book);
        /// <summary>
        /// Updates an existing book in the repository.
        /// </summary>
        Task<Book> UpdateAsync(Book book);
        /// <summary>
        /// Deletes a book by its ID.
        /// </summary>
        Task<bool> DeleteAsync(System.Guid id);
        /// <summary>
        /// Checks if a book with the given ISBN exists.
        /// </summary>
        Task<bool> ExistsAsync(string isbn);
    }
}
