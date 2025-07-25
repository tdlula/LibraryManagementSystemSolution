﻿using System;

namespace LibraryManagementSystem.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when a book is not found in the system.
    /// </summary>
    public class BookNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public BookNotFoundException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public BookNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
