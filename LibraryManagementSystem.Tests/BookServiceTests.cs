using NUnit.Framework;
using System;
using System.Threading.Tasks;
using LibraryManagementSystem.Service;
using LibraryManagementSystem.Common.DTOs;
using LibraryManagementSystem.Infrastructure.DAL;
using LibraryManagementSystem.Common.Exceptions;

namespace LibraryManagementSystem.Tests
{
    [TestFixture]
    public class BookServiceTests
    {
        private BookService _bookService;
        private InMemoryBookRepository _bookRepository;
        private UnitOfWork _unitOfWork;

        [SetUp]
        public void Setup()
        {
            _bookRepository = new InMemoryBookRepository();
            _unitOfWork = new UnitOfWork(_bookRepository);
            _bookService = new BookService(_unitOfWork);
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWork?.Dispose();
        }

        [Test]
        public async Task AddBookAsync_ShouldAddBook()
        {
            var dto = new CreateBookDto
            {
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "1234567890",
                PublicationYear = 2020
            };

            var result = await _bookService.AddBookAsync(dto);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo(dto.Title));
            Assert.That(result.Author, Is.EqualTo(dto.Author));
            Assert.That(result.ISBN, Is.EqualTo(dto.ISBN));
            Assert.That(result.PublicationYear, Is.EqualTo(dto.PublicationYear));
        }

        [Test]
        public async Task GetBookByIdAsync_ShouldReturnBook()
        {
            var dto = new CreateBookDto
            {
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "1234567891",
                PublicationYear = 2021
            };
            var added = await _bookService.AddBookAsync(dto);
            var result = await _bookService.GetBookByIdAsync(added.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(added.Id));
        }

        [Test]
        public async Task UpdateBookAsync_ShouldUpdateBook()
        {
            var dto = new CreateBookDto
            {
                Title = "Original Title",
                Author = "Original Author",
                ISBN = "1234567892",
                PublicationYear = 2022
            };
            var added = await _bookService.AddBookAsync(dto);
            var updateDto = new UpdateBookDto
            {
                Title = "Updated Title",
                Author = "Updated Author",
                ISBN = "1234567892",
                PublicationYear = 2023
            };
            var updated = await _bookService.UpdateBookAsync(added.Id, updateDto);
            Assert.That(updated.Title, Is.EqualTo("Updated Title"));
            Assert.That(updated.Author, Is.EqualTo("Updated Author"));
            Assert.That(updated.PublicationYear, Is.EqualTo(2023));
        }

        [Test]
        public async Task DeleteBookAsync_ShouldDeleteBook()
        {
            var dto = new CreateBookDto
            {
                Title = "To Delete",
                Author = "Author",
                ISBN = "1234567893",
                PublicationYear = 2024
            };
            var added = await _bookService.AddBookAsync(dto);
            var deleted = await _bookService.DeleteBookAsync(added.Id);
            Assert.That(deleted, Is.True);
            Assert.ThrowsAsync<BookNotFoundException>(async () => await _bookService.GetBookByIdAsync(added.Id));
        }

        [Test]
        public async Task SearchBooksAsync_ShouldReturnMatchingBooks()
        {
            await _bookService.AddBookAsync(new CreateBookDto
            {
                Title = "C# in Depth",
                Author = "Jon Skeet",
                ISBN = "1234567894",
                PublicationYear = 2019
            });
            var results = await _bookService.SearchBooksAsync("C#");
            Assert.That(results, Is.Not.Empty);
        }
    }
}
