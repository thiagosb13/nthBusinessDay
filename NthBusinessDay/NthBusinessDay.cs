using System;
using System.Collections.Generic;

namespace NthBusinessDay {
    public class NthBusinessDay {
        private ISet<DayOfWeek> RegularBusinessDays { get; }

        private IList<DateTime> holidays;
        public IList<DateTime> Holidays {
            get { return holidays ?? (holidays = new List<DateTime>());  }
            set { holidays = value; }
        }

        private IList<DateTime> daysToInclude;
        public IList<DateTime> DaysToInclude {
            get { return daysToInclude ?? (daysToInclude = new List<DateTime>());  }
            set { daysToInclude = value; }
        }

        public NthBusinessDay() {
            RegularBusinessDays = new SortedSet<DayOfWeek> {
                DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday
            };
        }

        /// <summary>
        /// Calculate the nth business days from a base date considering a range of holidays, 
        /// days of week which are business day (From Monday to Friday as default) and specific 
        /// included dates.
        /// </summary>
        /// <param name="baseDate"></param>
        /// <param name="nthDay"></param>
        /// <returns>nth business day as of baseDate</returns>
        /// <exception cref="BusinessDayOverlapsYearException"></exception>
        public DateTime AsOf(DateTime baseDate, int nthDay) {
            var businessDays = BusinessDays(baseDate);

            if (businessDays.Count - 1 < nthDay)
                throw new BusinessDayOverlapsYearException(nthDay - businessDays.Count + 1);

            return businessDays[nthDay];
        }

        public void AddDayOfWeek(DayOfWeek dayOfWeek) {
            RegularBusinessDays.Add(dayOfWeek);
        }

        public void RemoveDayOfWeek(DayOfWeek dayOfWeek) {
            RegularBusinessDays.Remove(dayOfWeek);
        }

        private IList<DateTime> BusinessDays(DateTime asOf) {
            var businessDays = new List<DateTime>();

            var dateToAdd = asOf;

            var firstDayOfNextYear = new DateTime(asOf.Year + 1, 1, 1);

            while (dateToAdd < firstDayOfNextYear) {
                if (DaysToInclude.Contains(dateToAdd) || (RegularBusinessDays.Contains(dateToAdd.DayOfWeek) && !Holidays.Contains(dateToAdd)))
                    businessDays.Add(dateToAdd);

                dateToAdd = dateToAdd.AddDays(1);
            }

            return businessDays;
        }
    }
}
