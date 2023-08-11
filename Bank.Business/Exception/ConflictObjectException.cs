namespace Bank.Business.Exception
{
    public class ConflictObjectException : BankException
    {
        public ConflictObjectException()
        {
        }

        public ConflictObjectException(string? message) : base(message)
        {
        }

        public ConflictObjectException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }
    }
}
