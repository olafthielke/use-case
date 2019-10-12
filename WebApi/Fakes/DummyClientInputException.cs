using BusinessLogic.Exceptions;

namespace WebApi.Fakes
{
    /// <summary>
    /// A derived ClientInputExcpetion class that only exists for
    /// unit testing a specific ClientInputException.
    /// </summary>
    public class DummyClientInputException : ClientInputException
    {
        public DummyClientInputException()
            : base()
        { }

        public DummyClientInputException(string message)
            : base(message)
        { }
    }
}