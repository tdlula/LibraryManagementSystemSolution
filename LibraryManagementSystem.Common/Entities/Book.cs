using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Common.Entities
{
    /// <summary>
    /// Represents a book entity with all required properties
    /// </summary>
    public class Book
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [StringLength(13)]
        public string ISBN { get; set; } = string.Empty;

        [Range(1800, 2100)]
        public int PublicationYear { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public override string ToString()
        {
            return $"{Title} by {Author} ({PublicationYear}) - ISBN: {ISBN}";
        }
    }
}
