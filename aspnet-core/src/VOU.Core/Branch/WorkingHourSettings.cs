using System;


namespace VOU.Branch
{
    public class WorkingHourSettings
    {
        /// <summary>
        /// The hour of the day to begin enforcement of this rule.
        /// </summary>
        public int Begin { get; set; }

        /// <summary>
        /// The hour of the day to end enforcement of this rule.
        /// </summary>
        public int End { get; set; }

        public WorkingHourSettings()
        {
            // EMPTY
        }

        public WorkingHourSettings(int begin, int end)
        {
            if (begin >= end)
                throw new ArgumentException($"{nameof(begin)} must be earlier than {nameof(end)} cannot both be null");

            Begin = begin;
            End = end;
        }
    }
}
