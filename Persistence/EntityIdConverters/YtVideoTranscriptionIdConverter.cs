using Domain.EntityIds;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.EntityIdConverters;

public sealed class YtVideoTranscriptionIdConverter : ValueConverter<YtVideoTranscriptionId, string>
{
    public YtVideoTranscriptionIdConverter()
        : base(id => id.Value.ToString(), value => new YtVideoTranscriptionId(value),
            new ConverterMappingHints(size: 26))
    {
    }
}