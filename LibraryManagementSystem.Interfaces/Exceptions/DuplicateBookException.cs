using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Common.Exceptions
{
    public class DuplicateBookException : Exception
    {
        public DuplicateBookException(string message) : base(message) { }
        public DuplicateBookException(string message, Exception innerException) : base(message, innerException) { }
    }

}
