using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Infrastructure.Models
{
    public enum AddVideoStatus
    {
        Success = 0,
        InsufficientCredits = 1,
        VideoAlreadyAdded = 2,
        Error = 100
    }
}
