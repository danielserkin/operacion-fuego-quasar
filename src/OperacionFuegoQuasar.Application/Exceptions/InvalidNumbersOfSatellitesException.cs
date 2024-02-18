namespace OperacionFuegoQuasar.Application.Exceptions
{
    public class InvalidNumbersOfSatellitesException : UserException
    {
        public InvalidNumbersOfSatellitesException() 
            : base("Exactly 3 satellites are required to determine the location and the message.")
        {
        }
    }
}
