using NUnit.Framework;
using System;
using System.Threading.Tasks;
using LibraryManagementSystem.Service;
using LibraryManagementSystem.Common.DTOs;
using LibraryManagementSystem.Infrastructure.DAL;
using LibraryManagementSystem.Common.Exceptions;

namespace LibraryManagementSystem.Tests
{
    /// <summary>
    /// Unit tests for the BookService class, covering CRUD and search operations.
    /// </summary>
    [TestFixture]
    public class BookServiceTests
    {
        private BookService _bookService;
        private InMemoryBookRepository _bookRepository;
        private UnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a fresh in-memory repository and service before each test.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _bookRepository = new InMemoryBookRepository();
            _unitOfWork = new UnitOfWork(_bookRepository);
            _bookService = new BookService(_unitOfWork);
        }

        /// <summary>
        /// Disposes the unit of work after each test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            _unitOfWork?.Dispose();
        }

        /// <summary>
        /// Tests that a book can be added and its properties are set correctly.
        /// </summary>
        [Test]
        public async Task AddBookAsync_ShouldAddBook()
        {
            // Arrange: create a new book DTO
            var dto = new CreateBookDto
            {
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "1234567890",
                PublicationYear = 2020
            };

            // Act: add the book
            var result = await _bookService.AddBookAsync(dto);

            // Assert: verify the book was added and properties match
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo(dto.Title));
            Assert.That(result.Author, Is.EqualTo(dto.Author));
            Assert.That(result.ISBN, Is.EqualTo(dto.ISBN));
            Assert.That(result.PublicationYear, Is.EqualTo(dto.PublicationYear));
        }

        /// <summary>
        /// Tests that a book can be retrieved by its ID after being added.
        /// </summary>
        [Test]
        public async Task GetBookByIdAsync_ShouldReturnBook()
        {
            // Arrange: add a book
            var dto = new CreateBookDto
            {
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "1234567891",
                PublicationYear = 2021
            };
            var added = await _bookService.AddBookAsync(dto);

            // Act: retrieve the book by ID
            var result = await _bookService.GetBookByIdAsync(added.Id);

            // Assert: verify the book is found and IDs match
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(added.Id));
        }

        /// <summary>
        /// Tests that updating a book changes its properties as expected.
        /// </summary>
        [Test]
        public async Task UpdateBookAsync_ShouldUpdateBook()
        {
            // Arrange: add a book
            var dto = new CreateBookDto
            {
                Title = "Original Title",
                Author = "Original Author",
                ISBN = "1234567892",
                PublicationYear = 2022
            };
            var added = await _bookService.AddBookAsync(dto);

            // Prepare update DTO with new values
            var updateDto = new UpdateBookDto
            {
                Title = "Updated Title",
                Author = "Updated Author",
                ISBN = "1234567892",
                PublicationYear = 2023
            };

            // Act: update the book
            var updated = await _bookService.UpdateBookAsync(added.Id, updateDto);

            // Assert: verify the book's properties were updated
            Assert.That(updated.Title, Is.EqualTo("Updated Title"));
            Assert.That(updated.Author, Is.EqualTo("Updated Author"));
            Assert.That(updated.PublicationYear, Is.EqualTo(2023));
        }

        /// <summary>
        /// Tests that a book can be deleted and is no longer retrievable.
        /// </summary>
        [Test]
        public async Task DeleteBookAsync_ShouldDeleteBook()
        {
            // Arrange: add a book
            var dto = new CreateBookDto
            {
                Title = "To Delete",
                Author = "Author",
                ISBN = "1234567893",
                PublicationYear = 2024
            };
            var added = await _bookService.AddBookAsync(dto);

            // Act: delete the book
            var deleted = await _bookService.DeleteBookAsync(added.Id);

            // Assert: verify deletion and that the book cannot be found
            Assert.That(deleted, Is.True);
            Assert.ThrowsAsync<BookNotFoundException>(async () => await _bookService.GetBookByIdAsync(added.Id));
        }

        /// <summary>
        /// Tests that searching for a term returns matching books.
        /// </summary>
        [Test]
        public async Task SearchBooksAsync_ShouldReturnMatchingBooks()
        {
            // Arrange: add a book with a unique title
            await _bookService.AddBookAsync(new CreateBookDto
            {
                Title = "C# in Depth",
                Author = "Jon Skeet",
                ISBN = "1234567894",
                PublicationYear = 2019
            });

            // Act: search for the book by part of its title
            var results = await _bookService.SearchBooksAsync("C#");

            // Assert: verify that at least one result is returned
            Assert.That(results, Is.Not.Empty);
        }

        /// <summary>
        /// Tests that adding a book with invalid data throws a ValidationException.
        /// </summary>
        [Test]
        public void AddBookAsync_WithInvalidData_ShouldThrowValidationException()
        {
            // Arrange: create a book DTO with invalid (empty) title and invalid ISBN
            var invalidDto = new CreateBookDto
            {
                Title = "", // Invalid: empty title
                Author = "Test Author",
                ISBN = "invalidisbn", // Invalid: not 10 or 13 digits
                PublicationYear = 2020
            };

            // Act & Assert: adding should throw ValidationException
            Assert.ThrowsAsync<ValidationException>(async () => await _bookService.AddBookAsync(invalidDto));
        }
    }
}
