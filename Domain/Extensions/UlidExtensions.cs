namespace Domain.Extensions;

public static class UlidExtensions
{
    public static Ulid ToUlid(this string ulid) => Ulid.Parse(ulid);
}