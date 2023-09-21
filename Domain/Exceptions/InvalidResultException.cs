namespace Domain.Exceptions;

public sealed class InvalidResultException : Exception
{
    public InvalidResultException(string errorMessage) : base(errorMessage)
    {
    }
}