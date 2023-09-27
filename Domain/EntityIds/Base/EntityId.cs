namespace Domain.EntityIds.Base;

public abstract record EntityId
{
    [Newtonsoft.Json.JsonProperty, System.Text.Json.Serialization.JsonInclude]
    public Ulid Value { get; private set; }

    protected EntityId()
    {
        Value = Ulid.NewUlid();
    }

    protected EntityId(string ulid)
    {
        Value = Ulid.Parse(ulid);
    }

    public static implicit operator string(EntityId id )=> id.ToString();
    public override string ToString() => Value.ToString();
}