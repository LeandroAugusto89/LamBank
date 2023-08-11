
namespace Bank.Business.Exception
{

    public class BankException : System.Exception
    {
        public BankException()
        {
        }

        public BankException(string? message) : base(message)
        {
        }

        public BankException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }
    }
}
