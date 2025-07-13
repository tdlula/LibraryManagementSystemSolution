using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Common.DTOs
{
    /// <summary>
    /// Data Transfer Object for creating a new book.
    /// </summary>
    public class CreateBookDto
    {
        /// <summary>
        /// Title of the book.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Author of the book.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Author { get; set; } = string.Empty;
        /// <summary>
        /// ISBN of the book.
        /// </summary>
        [Required]
        [StringLength(13)]
        public string ISBN { get; set; } = string.Empty;
        /// <summary>
        /// Year the book was published.
        /// </summary>
        [Range(1800, 2100)]
        public int PublicationYear { get; set; }
    }
}
