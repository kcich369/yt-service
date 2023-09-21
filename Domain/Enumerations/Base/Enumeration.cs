using System.Reflection;

namespace Domain.Enumerations.Base;

public class Enumeration: IComparable
{
    public string Name { get; private set; }
    public int Id { get; private set; }

    protected Enumeration(int id, string name) => (Id, Name) = (id, name);

    public override string ToString() => Name;

    public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();

    public override bool Equals(object obj)
    {
        if (obj is not Enumeration otherValue)
        {
            return false;
        }

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = Id.Equals(otherValue.Id);

        return typeMatches && valueMatches;
    }

    public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);

    public bool Contains(params Enumeration[] enumerations) => enumerations.Contains(this);

    public static implicit operator int(Enumeration enumeration) => enumeration.Id;
    
    public static bool operator ==(Enumeration enum1, Enumeration enum2) => enum1!.Id == enum2!.Id;

    public static bool operator !=(Enumeration enum1, Enumeration enum2) => enum1!.Id != enum2!.Id;
}