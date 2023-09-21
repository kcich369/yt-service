using Domain.EntityIds;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.EntityIdConverters;

public class YtVideoTagIdConverter: ValueConverter<YtVideoTagId, string>
{
    public YtVideoTagIdConverter()
        : base(id => id.Value.ToString(), value => new YtVideoTagId(value),
            new ConverterMappingHints(size: 26))
    {
    }
}