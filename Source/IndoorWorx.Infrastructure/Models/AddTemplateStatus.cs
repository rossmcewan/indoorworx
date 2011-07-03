using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Infrastructure.Models
{
    public enum AddTemplateStatus
    {
        Success = 0,
        InsufficientCredits = 1,
        TemplateAlreadyAdded = 2,
        Error = 100
    }
}
