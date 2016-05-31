using System;

namespace NthBusinessDay {
    public class BusinessDayOverlapsYearException : Exception {
        public int RemainingDays { get; private set; }

        public BusinessDayOverlapsYearException(int remaingDays) {
            RemainingDays = remaingDays;
        }
    }
}
