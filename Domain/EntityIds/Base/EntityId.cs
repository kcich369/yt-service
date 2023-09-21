using Newtonsoft.Json;

namespace Domain.EntityIds.Base;

public abstract record EntityId
{
    [JsonProperty]
    public Ulid Value { get; protected set; }

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