using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniWeb.API.Models
{
    public class TimeSlot
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}
