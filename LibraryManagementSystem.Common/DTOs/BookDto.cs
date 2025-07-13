using System;

namespace LibraryManagementSystem.Common.DTOs
{
    /// <summary>
    /// Data Transfer Object for book operations. Used to return book data to the UI layer.
    /// </summary>
    public class BookDto
    {
        /// <summary>
        /// Unique identifier for the book.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Title of the book.
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Author of the book.
        /// </summary>
        public string Author { get; set; } = string.Empty;
        /// <summary>
        /// ISBN of the book.
        /// </summary>
        public string ISBN { get; set; } = string.Empty;
        /// <summary>
        /// Year the book was published.
        /// </summary>
        public int PublicationYear { get; set; }
        /// <summary>
        /// Date and time the book was created in the system.
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Date and time the book was last updated in the system.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
