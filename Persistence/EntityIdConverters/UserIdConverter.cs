using Domain.EntityIds;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.EntityIdConverters;

public class UserIdConverter : ValueConverter<UserId, string>
{
    public UserIdConverter()
        : base(id => id.Value.ToString(), value => new UserId(value),
            new ConverterMappingHints(size: 26))
    {
    }
}