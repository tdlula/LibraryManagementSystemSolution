﻿using System;

namespace LibraryManagementSystem.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to add a duplicate book (by ISBN).
    /// </summary>
    public class DuplicateBookException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateBookException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public DuplicateBookException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateBookException"/> class with a 
        /// specified error message and a reference to the inner exception that is the cause of 
        /// this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, 
        /// or a null reference if no inner exception is specified.</param>
        public DuplicateBookException(string message, Exception innerException) : base(message, innerException) { }
    }

}
