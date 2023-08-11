
namespace Bank.Business.Exception
{
    public class InvalidObjectException : BankException
    {
        public InvalidObjectException()
        {
        }

        public InvalidObjectException(string? message) : base(message)
        {
        }

        public InvalidObjectException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }
    }
}
