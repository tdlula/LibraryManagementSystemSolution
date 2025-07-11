using LibraryManagementSystem.Common.DTOs;
using LibraryManagementSystem.Common.Exceptions;
using LibraryManagementSystem.Common.Interfaces;
using LibraryManagementSystem.Infrastructure.DAL;
using LibraryManagementSystem.Service;
using System.Text.RegularExpressions;

public class Program
{
    // Service for book operations
    private static IBookService _bookService;

    // Sanitizes user input: trims, checks for null, and removes control characters
    private static string SanitizeInput(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;
        // Remove control characters except for whitespace
        return Regex.Replace(input.Trim(), "[\x00-\x1F\x7F]", string.Empty);
    }

    // Main method: application entry point
    public static async Task Main(string[] args)
    {
        // Dependency injection setup: create repository, unit of work, and service
        var bookRepository = new InMemoryBookRepository();
        var unitOfWork = new UnitOfWork(bookRepository);
        _bookService = new BookService(unitOfWork);

        // Seed the system with sample data
        await SeedSampleData();

        Console.WriteLine("=== Library Management System ===");
        Console.WriteLine("Enterprise-level implementation with Clean Architecture");
        Console.WriteLine();

        // Main menu loop
        while (true)
        {
            DisplayMenu();
            var choice = SanitizeInput(Console.ReadLine());

            try
            {
                // Handle user menu selection
                switch (choice)
                {
                    case "1":
                        await DisplayAllBooks();
                        break;
                    case "2":
                        await SearchBooks();
                        break;
                    case "3":
                        await AddBook();
                        break;
                    case "4":
                        await UpdateBook();
                        break;
                    case "5":
                        await DeleteBook();
                        break;
                    case "6":
                        await GetBookById();
                        break;
                    case "7":
                        await GetBookByISBN();
                        break;
                    case "0":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                // Display any errors encountered during operation
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    private static void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Library Management System ===");
        Console.WriteLine("1. Display All Books");
        Console.WriteLine("2. Search Books");
        Console.WriteLine("3. Add Book");
        Console.WriteLine("4. Update Book");
        Console.WriteLine("5. Delete Book");
        Console.WriteLine("6. Get Book by ID");
        Console.WriteLine("7. Get Book by ISBN");
        Console.WriteLine("0. Exit");
        Console.Write("\nEnter your choice: ");
    }

    private static async Task DisplayAllBooks()
    {
        Console.Clear();
        Console.WriteLine("=== All Books ===");

        var books = await _bookService.GetAllBooksAsync();
        if (!books.Any())
        {
            Console.WriteLine("No books found.");
            return;
        }

        foreach (var book in books)
        {
            DisplayBook(book);
        }
    }

    private static async Task SearchBooks()
    {
        Console.Clear();
        Console.WriteLine("=== Search Books ===");
        Console.Write("Enter search term (title, author, or ISBN): ");
        var searchTerm = SanitizeInput(Console.ReadLine());

        var books = await _bookService.SearchBooksAsync(searchTerm);
        if (!books.Any())
        {
            Console.WriteLine("No books found matching your search.");
            return;
        }

        Console.WriteLine($"\nFound {books.Count()} book(s):");
        foreach (var book in books)
        {
            DisplayBook(book);
        }
    }

    // Prompts user for book details and adds a new book
    private static async Task AddBook()
    {
        Console.Clear();
        Console.WriteLine("=== Add New Book ===");

        var createBookDto = new CreateBookDto();

        Console.Write("Title: ");
        createBookDto.Title = SanitizeInput(Console.ReadLine());

        Console.Write("Author: ");
        createBookDto.Author = SanitizeInput(Console.ReadLine());

        Console.Write("ISBN: ");
        createBookDto.ISBN = SanitizeInput(Console.ReadLine());

        Console.Write("Publication Year: ");
        var yearInput = SanitizeInput(Console.ReadLine());
        if (int.TryParse(yearInput, out var year) && year > 0 && year < 3000)
        {
            createBookDto.PublicationYear = year;
        }
        else
        {
            Console.WriteLine("Invalid publication year. Book not added.");
            return;
        }

        var addedBook = await _bookService.AddBookAsync(createBookDto);
        Console.WriteLine($"\nBook added successfully:");
        DisplayBook(addedBook);
    }

    // Prompts user for book ID and new details, then updates the book
    private static async Task UpdateBook()
    {
        Console.Clear();
        Console.WriteLine("=== Update Book ===");

        Console.Write("Enter book ID to update: ");
        var idInput = SanitizeInput(Console.ReadLine());
        if (!Guid.TryParse(idInput, out var id))
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        var existingBook = await _bookService.GetBookByIdAsync(id);
        Console.WriteLine($"\nCurrent book details:");
        DisplayBook(existingBook);

        var updateBookDto = new UpdateBookDto();

        // Prompt for new values, defaulting to existing if left blank
        Console.Write($"Title ({existingBook.Title}): ");
        var title = SanitizeInput(Console.ReadLine());
        updateBookDto.Title = string.IsNullOrEmpty(title) ? existingBook.Title : title;

        Console.Write($"Author ({existingBook.Author}): ");
        var author = SanitizeInput(Console.ReadLine());
        updateBookDto.Author = string.IsNullOrEmpty(author) ? existingBook.Author : author;

        Console.Write($"ISBN ({existingBook.ISBN}): ");
        var isbn = SanitizeInput(Console.ReadLine());
        updateBookDto.ISBN = string.IsNullOrEmpty(isbn) ? existingBook.ISBN : isbn;

        Console.Write($"Publication Year ({existingBook.PublicationYear}): ");
        var yearInput = SanitizeInput(Console.ReadLine());
        if (string.IsNullOrEmpty(yearInput))
        {
            updateBookDto.PublicationYear = existingBook.PublicationYear;
        }
        else if (int.TryParse(yearInput, out var year) && year > 0 && year < 3000)
        {
            updateBookDto.PublicationYear = year;
        }
        else
        {
            Console.WriteLine("Invalid publication year. Book not updated.");
            return;
        }

        var updatedBook = await _bookService.UpdateBookAsync(id, updateBookDto);
        Console.WriteLine($"\nBook updated successfully:");
        DisplayBook(updatedBook);
    }

    // Prompts user for book ID and deletes the book after confirmation
    private static async Task DeleteBook()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Book ===");

        Console.Write("Enter book ID to delete: ");
        var idInput = SanitizeInput(Console.ReadLine());
        if (!Guid.TryParse(idInput, out var id))
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        var book = await _bookService.GetBookByIdAsync(id);
        Console.WriteLine($"\nBook to delete:");
        DisplayBook(book);

        Console.Write("\nAre you sure you want to delete this book? (y/N): ");
        var confirmation = SanitizeInput(Console.ReadLine());

        if (confirmation.ToLower() == "y")
        {
            await _bookService.DeleteBookAsync(id);
            Console.WriteLine("Book deleted successfully.");
        }
        else
        {
            Console.WriteLine("Delete cancelled.");
        }
    }

    // Gets and displays a book by its ID
    private static async Task GetBookById()
    {
        Console.Clear();
        Console.WriteLine("=== Get Book by ID ===");

        Console.Write("Enter book ID: ");
        var idInput = SanitizeInput(Console.ReadLine());
        if (!Guid.TryParse(idInput, out var id))
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        var book = await _bookService.GetBookByIdAsync(id);
        DisplayBook(book);
    }

    // Gets and displays a book by its ISBN
    private static async Task GetBookByISBN()
    {
        Console.Clear();
        Console.WriteLine("=== Get Book by ISBN ===");

        Console.Write("Enter ISBN: ");
        var isbn = SanitizeInput(Console.ReadLine());

        var book = await _bookService.GetBookByISBNAsync(isbn);
        DisplayBook(book);
    }

    // Displays details of a single book
    private static void DisplayBook(BookDto book)
    {
        Console.WriteLine($"ID: {book.Id}");
        Console.WriteLine($"Title: {book.Title}");
        Console.WriteLine($"Author: {book.Author}");
        Console.WriteLine($"ISBN: {book.ISBN}");
        Console.WriteLine($"Publication Year: {book.PublicationYear}");
        Console.WriteLine($"Created: {book.CreatedAt:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine($"Updated: {book.UpdatedAt:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine(new string('-', 50));
    }

    // Seeds the system with a set of sample books
    private static async Task SeedSampleData()
    {
        var sampleBooks = new[]
        {
                new CreateBookDto
                {
                    Title = "Clean Code",
                    Author = "Robert C. Martin",
                    ISBN = "9780132350884",
                    PublicationYear = 2008
                },
                new CreateBookDto
                {
                    Title = "Design Patterns",
                    Author = "Gang of Four",
                    ISBN = "9780201633612",
                    PublicationYear = 1994
                },
                new CreateBookDto
                {
                    Title = "Clean Architecture",
                    Author = "Robert C. Martin",
                    ISBN = "9780134494166",
                    PublicationYear = 2017
                }
            };

        foreach (var book in sampleBooks)
        {
            try
            {
                await _bookService.AddBookAsync(book);
            }
            catch (DuplicateBookException)
            {
                // Ignore duplicates during seeding
            }
        }
    }
}
