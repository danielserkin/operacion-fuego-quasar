
namespace OperacionFuegoQuasar.Application.Exceptions
{
    public class InvalidNumbersOfDistancesException : UserException
    {
        public InvalidNumbersOfDistancesException() : base("Exactly 3 distances are required for triangulation.")
        {
        }
    }
}
