using Domain.EntityIds;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.EntityIdConverters;

public sealed class YtChannelIdConverter : ValueConverter<YtChannelId, string>
{
    public YtChannelIdConverter()
        : base(id => id.Value.ToString(), value => new YtChannelId(value),
            new ConverterMappingHints(size: 26))
    {
    }
}