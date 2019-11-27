using System;
using System.Collections.Generic;


namespace VOU.Branch
{
    [Serializable]
    public class TimeTableSettings
    {
        public List<TimeTable> TimeTables { get; set; }
           = new List<TimeTable>();
    }
}
