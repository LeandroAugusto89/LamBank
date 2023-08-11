namespace Bank.Business.Exception
{
    public class ExistingObjectException : BankException
    {
        public ExistingObjectException()
        {
        }

        public ExistingObjectException(string? message) : base(message)
        {
        }

        public ExistingObjectException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }
    }
}
