using Domain.Entities;

namespace Domain.Comparers;

public sealed class YtVideoComparer : IEqualityComparer<YtVideo>
{
    public bool Equals(YtVideo x, YtVideo y)
    {
        if (x is null && y is null)
            return true;
        if (x is null || y is null)
            return false;
        return x.YtId == y.YtId;
    }

    public int GetHashCode(YtVideo obj) => obj.YtId.GetHashCode();
}