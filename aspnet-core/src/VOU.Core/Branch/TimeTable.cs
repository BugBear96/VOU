
using System;
using System.Collections.Generic;

namespace VOU.Branch
{
   public  class TimeTable
    {
        public TimeTable(
           string key,
            List<DayOfWeek> days,
            List<WorkingHourSettings> workingHours)
        {
            Days = days ?? new List<DayOfWeek>();
            WorkingHours = workingHours ?? new List<WorkingHourSettings>();
        }

        public List<DayOfWeek> Days { get; set; }
            = new List<DayOfWeek>();

        public List<WorkingHourSettings> WorkingHours { get; set; }
            = new List<WorkingHourSettings>();
    }
}
