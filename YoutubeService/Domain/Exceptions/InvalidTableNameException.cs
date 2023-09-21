namespace Domain.Exceptions;

public class InvalidTableNameException : Exception
{
    public InvalidTableNameException(string message) : base(message)
    {
        
    }
}