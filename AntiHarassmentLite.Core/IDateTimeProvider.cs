using System;

namespace AntiHarassmentLite.Core
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
