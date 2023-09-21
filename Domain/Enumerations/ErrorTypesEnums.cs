using Domain.Enumerations.Base;

namespace Domain.Enumerations;

public sealed class ErrorTypesEnums : Enumeration
{
    public static ErrorTypesEnums NotFound = new(1, nameof(NotFound));
    public static ErrorTypesEnums BadRequest = new(2, nameof(BadRequest));
    public static ErrorTypesEnums Validation = new(3, nameof(Validation));
    public static ErrorTypesEnums Exception = new(4, nameof(Exception));

    private ErrorTypesEnums(int id, string name) : base(id, name)
    {
    }
}