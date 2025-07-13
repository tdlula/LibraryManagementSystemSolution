using LibraryManagementSystem.Common.DTOs;
using LibraryManagementSystem.Common.Exceptions;
using LibraryManagementSystem.Common.Interfaces;
using LibraryManagementSystem.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Service
{
    /// <summary>
    /// Business logic service for book operations. Handles CRUD and search operations for books.
    /// </summary>
    public class BookService : IBookService
    {
        // Unit of work for repository and transaction management
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor for BookService
        /// </summary>
        /// <param name="unitOfWork">Unit of work instance</param>
        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// Gets all books in the system.
        /// </summary>
        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            var books = await _unitOfWork.Books.GetAllAsync();
            return books.Select(MapToDto);
        }

        /// <summary>
        /// Gets a book by its unique ID.
        /// </summary>
        public async Task<BookDto> GetBookByIdAsync(Guid id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);
            if (book is null) // Use 'is null' instead of '== null' for nullable types
                throw new BookNotFoundException($"Book with ID {id} not found.");

            return MapToDto(book);
        }

        /// <summary>
        /// Gets a book by its ISBN.
        /// </summary>
        public async Task<BookDto> GetBookByISBNAsync(string isbn)
        {
            ValidateISBN(isbn);
            var book = await _unitOfWork.Books.GetByISBNAsync(isbn);
            if (book is null) // Use 'is null' instead of '== null' for nullable types
                throw new BookNotFoundException($"Book with ISBN {isbn} not found.");

            return MapToDto(book);
        }

        /// <summary>
        /// Searches for books by a search term (title, author, or ISBN).
        /// </summary>
        public async Task<IEnumerable<BookDto>> SearchBooksAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllBooksAsync();

            var books = await _unitOfWork.Books.SearchAsync(searchTerm.Trim());
            return books.Select(MapToDto);
        }

        /// <summary>
        /// Adds a new book to the system.
        /// </summary>
        public async Task<BookDto> AddBookAsync(CreateBookDto createBookDto)
        {
            ValidateCreateBookDto(createBookDto);

            // Check for duplicate ISBN
            if (await _unitOfWork.Books.ExistsAsync(createBookDto.ISBN))
                throw new DuplicateBookException($"Book with ISBN {createBookDto.ISBN} already exists.");

            var book = new Common.Entities.Book
            {
                Title = createBookDto.Title.Trim(),
                Author = createBookDto.Author.Trim(),
                ISBN = createBookDto.ISBN.Trim(),
                PublicationYear = createBookDto.PublicationYear
            };

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var addedBook = await _unitOfWork.Books.AddAsync(book);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return MapToDto(addedBook);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        /// <summary>
        /// Updates an existing book's details.
        /// </summary>
        public async Task<BookDto> UpdateBookAsync(Guid id, UpdateBookDto updateBookDto)
        {
            ValidateUpdateBookDto(updateBookDto);

            var existingBook = await _unitOfWork.Books.GetByIdAsync(id);
            if (existingBook is null) // Use 'is null' instead of '== null' for nullable types
                throw new BookNotFoundException($"Book with ID {id} not found.");

            // Check for duplicate ISBN (excluding current book)
            var bookWithSameISBN = await _unitOfWork.Books.GetByISBNAsync(updateBookDto.ISBN);
            if (bookWithSameISBN is not null && bookWithSameISBN.Id != id) // Use 'is not null' instead of '!= null'
                throw new DuplicateBookException($"Another book with ISBN {updateBookDto.ISBN} already exists.");

            existingBook.Title = updateBookDto.Title.Trim();
            existingBook.Author = updateBookDto.Author.Trim();
            existingBook.ISBN = updateBookDto.ISBN.Trim();
            existingBook.PublicationYear = updateBookDto.PublicationYear;
            existingBook.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var updatedBook = await _unitOfWork.Books.UpdateAsync(existingBook);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return MapToDto(updatedBook);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        /// <summary>
        /// Deletes a book by its ID.
        /// </summary>
        public async Task<bool> DeleteBookAsync(Guid id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);
            if (book is null) 
                throw new BookNotFoundException($"Book with ID {id} not found.");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _unitOfWork.Books.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return result;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        /// <summary>
        /// Maps a Book entity to a BookDto.
        /// </summary>
        private static BookDto MapToDto(Common.Entities.Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                PublicationYear = book.PublicationYear,
                CreatedAt = book.CreatedAt,
                UpdatedAt = book.UpdatedAt
            };
        }

        /// <summary>
        /// Validates a CreateBookDto using data annotations and custom logic.
        /// </summary>
        private static void ValidateCreateBookDto(CreateBookDto dto)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(dto);

            if (!Validator.TryValidateObject(dto, context, validationResults, true))
            {
                var errors = string.Join(", ", validationResults.Select(r => r.ErrorMessage));
                throw new Common.Exceptions.ValidationException($"Validation failed: {errors}");
            }

            ValidateISBN(dto.ISBN);
        }

        /// <summary>
        /// Validates an UpdateBookDto using data annotations and custom logic.
        /// </summary>
        private static void ValidateUpdateBookDto(UpdateBookDto dto)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(dto);

            if (!Validator.TryValidateObject(dto, context, validationResults, true))
            {
                var errors = string.Join(", ", validationResults.Select(r => r.ErrorMessage));
                throw new Common.Exceptions.ValidationException($"Validation failed: {errors}");
            }

            ValidateISBN(dto.ISBN);
        }

        /// <summary>
        /// Validates an ISBN for correct format and length.
        /// </summary>
        private static void ValidateISBN(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                throw new Common.Exceptions.ValidationException("ISBN cannot be empty.");

            var cleanIsbn = isbn.Replace("-", "").Replace(" ", "");
            if (cleanIsbn.Length != 10 && cleanIsbn.Length != 13)
                throw new Common.Exceptions.ValidationException("ISBN must be 10 or 13 characters long.");

            if (!cleanIsbn.All(char.IsDigit))
                throw new Common.Exceptions.ValidationException("ISBN must contain only digits.");
        }
    }
}
