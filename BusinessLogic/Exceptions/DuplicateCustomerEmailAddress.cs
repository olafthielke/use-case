namespace BusinessLogic.Exceptions
{
    public class DuplicateCustomerEmailAddress : ClientInputException
    {
        public string EmailAddress { get; private set; }

        public DuplicateCustomerEmailAddress(string emailAddress)
            : base($"The email address '{emailAddress}' already exists in the system.")
        {
            EmailAddress = emailAddress;
        }
    }
}
