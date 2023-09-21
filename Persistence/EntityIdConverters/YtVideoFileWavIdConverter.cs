using Domain.EntityIds;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.EntityIdConverters;

public sealed class YtVideoFileWavIdConverter : ValueConverter<YtVideoFileWavId, string>
{
    public YtVideoFileWavIdConverter()
        : base(id => id.Value.ToString(), value => new YtVideoFileWavId(value),
            new ConverterMappingHints(size: 26))
    {
    }
}