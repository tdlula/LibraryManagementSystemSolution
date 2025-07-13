using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagementSystem.Common.DTOs;

namespace LibraryManagementSystem.Common.Interfaces
{
    /// <summary>
    /// Service interface for book operations.
    /// </summary>
    public interface IBookService
    {
        /// <summary>
        /// Gets all books in the system.
        /// </summary>
        Task<IEnumerable<BookDto>> GetAllBooksAsync();
        /// <summary>
        /// Gets a book by its unique ID.
        /// </summary>
        Task<BookDto> GetBookByIdAsync(Guid id);
        /// <summary>
        /// Gets a book by its ISBN.
        /// </summary>
        Task<BookDto> GetBookByISBNAsync(string isbn);
        /// <summary>
        /// Searches for books by a search term (title, author, or ISBN).
        /// </summary>
        Task<IEnumerable<BookDto>> SearchBooksAsync(string searchTerm);
        /// <summary>
        /// Adds a new book to the system.
        /// </summary>
        Task<BookDto> AddBookAsync(CreateBookDto createBookDto);
        /// <summary>
        /// Updates an existing book's details.
        /// </summary>
        Task<BookDto> UpdateBookAsync(Guid id, UpdateBookDto updateBookDto);
        /// <summary>
        /// Deletes a book by its ID.
        /// </summary>
        Task<bool> DeleteBookAsync(Guid id);
    }
}
