using LibraryManagementSystem.Common.Entities;


namespace LibraryManagementSystem.Common.Interfaces
{
    /// <summary>
    /// Repository pattern interface for book operations
    /// </summary>
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(Guid id);
        Task<Book?> GetByISBNAsync(string isbn);
        Task<IEnumerable<Book>> SearchAsync(string searchTerm);
        Task<Book> AddAsync(Book book);
        Task<Book> UpdateAsync(Book book);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(string isbn);
    }
}
