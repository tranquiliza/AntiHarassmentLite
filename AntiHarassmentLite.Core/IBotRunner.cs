using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AntiHarassmentLite.Core
{
    public interface IBotRunner : IDisposable
    {
        Task InitializeAsync();
        Task JoinChannels();
    }
}
