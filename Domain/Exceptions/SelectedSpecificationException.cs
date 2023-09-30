namespace Domain.Exceptions;

public sealed class SelectedSpecificationException : Exception
{
    public SelectedSpecificationException(string message) : base(message)
    {
    }
}