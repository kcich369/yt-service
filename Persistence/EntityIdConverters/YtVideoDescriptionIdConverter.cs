using Domain.EntityIds;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.EntityIdConverters;

public sealed class YtVideoDescriptionIdConverter : ValueConverter<YtVideoDescriptionId, string>
{
    public YtVideoDescriptionIdConverter()
        : base(id => id.Value.ToString(), value => new YtVideoDescriptionId(value),
            new ConverterMappingHints(size: 26))
    {
    }
}