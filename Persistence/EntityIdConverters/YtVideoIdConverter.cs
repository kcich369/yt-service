using Domain.EntityIds;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.EntityIdConverters;

public sealed class YtVideoIdConverter : ValueConverter<YtVideoId, string>
{
    public YtVideoIdConverter()
        : base(id => id.Value.ToString(), value => new YtVideoId(value),
            new ConverterMappingHints(size: 26))
    {
    }
}