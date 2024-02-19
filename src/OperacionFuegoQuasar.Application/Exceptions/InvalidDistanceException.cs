namespace OperacionFuegoQuasar.Application.Exceptions;

public class InvalidDistanceException : UserException
{
    public InvalidDistanceException() : base("There is an incorrect distance.")
    {
    }
}
