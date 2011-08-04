using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndoorWorx.Infrastructure.Helpers
{
    public class DateTimeHelper
    {
        private static DateTime zero = new DateTime(1971, 1, 1, 0, 0, 0);
        public static DateTime ZeroTime
        {
            get { return zero; }
        }
    }
}
