﻿
namespace OperacionFuegoQuasar.Infrastructure.Exceptions;

public class DbOperationException : InfrastructureException
{
    public DbOperationException() : base("An error occurred while trying to perform the operation on the database.")
    {
    }
}
