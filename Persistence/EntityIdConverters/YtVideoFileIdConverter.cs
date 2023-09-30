using Domain.Entities;
using Domain.EntityIds;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.EntityIdConverters;

public sealed class YtVideoFileIdConverter : ValueConverter<YtVideoFileId, string>
{
    public YtVideoFileIdConverter()
        : base(id => id.Value.ToString(), value => new YtVideoFileId(value),
            new ConverterMappingHints(size: 26))
    {
    }
}