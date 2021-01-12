using AntiHarassmentLite.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AntiHarassmentLite.Core
{
    public interface ISuspensionRepository
    {
        Task SaveSuspension(Suspension suspension);
        Task<List<Suspension>> GetSuspensions(string username);
    }
}
