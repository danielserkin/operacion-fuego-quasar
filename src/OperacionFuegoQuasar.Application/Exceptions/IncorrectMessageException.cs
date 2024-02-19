namespace OperacionFuegoQuasar.Application.Exceptions;

public class IncorrectMessageException : UserException
{
    public IncorrectMessageException() : base("There is an incorrect format message.")
    {
    }
}