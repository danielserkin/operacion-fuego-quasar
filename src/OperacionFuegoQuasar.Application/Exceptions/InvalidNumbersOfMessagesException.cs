
namespace OperacionFuegoQuasar.Application.Exceptions
{
    public class InvalidNumbersOfMessagesException : UserException
    {
        public InvalidNumbersOfMessagesException() : base("At least three messages are required to construct the final message.")
        {
        }
    }
}
