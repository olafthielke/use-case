using System;

namespace BusinessLogic.Exceptions
{
    /// <summary>
    /// Base exception class for situations when the client sent
    /// invalid input data. 
    /// Particularly useful in web context. Can catch this and emit
    /// 400 - Bad Request HTTP status code.
    /// Derive specific child exceptions. For example, MissingEmailAddress.
    /// </summary>
    public abstract class ClientInputException : Exception
    {
        public ClientInputException()
            : base()
        { }

        public ClientInputException(string message)
            : base(message)
        { }
    }
}
