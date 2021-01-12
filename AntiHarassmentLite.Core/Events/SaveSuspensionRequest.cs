using AntiHarassmentLite.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntiHarassmentLite.Core.Events
{
    public class SaveSuspensionRequest : IRequest
    {
        public Suspension Suspension { get; set; }

        public SaveSuspensionRequest(Suspension suspension)
        {
            Suspension = suspension;
        }
    }
}
