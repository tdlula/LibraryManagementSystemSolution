using LibraryManagementSystem.Common.DTOs;

namespace LibraryManagementSystem.Common.Interfaces
{
    /// <summary>
    /// Service interface for book operations
    /// </summary>
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllBooksAsync();
        Task<BookDto> GetBookByIdAsync(Guid id);
        Task<BookDto> GetBookByISBNAsync(string isbn);
        Task<IEnumerable<BookDto>> SearchBooksAsync(string searchTerm);
        Task<BookDto> AddBookAsync(CreateBookDto createBookDto);
        Task<BookDto> UpdateBookAsync(Guid id, UpdateBookDto updateBookDto);
        Task<bool> DeleteBookAsync(Guid id);
    }
}
