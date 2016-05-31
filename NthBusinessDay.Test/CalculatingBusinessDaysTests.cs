using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace NthBusinessDay.Test {
    [TestFixture]
    public class CalculatingBusinessDaysTests {
        [Test]
        public void ShouldReturnsNthBusinessDay() {
            var nthBusinessDay = new NthBusinessDay();

            var day = nthBusinessDay.AsOf(new DateTime(2016, 5, 30), 3);

            Assert.That(day, Is.EqualTo(new DateTime(2016, 6, 2)));
        }

        [Test]
        public void ShouldReturnsNthBusinessDayConsideringDaysOfWeekSettings() {
            var nthBusinessDay = new NthBusinessDay();

            nthBusinessDay.RemoveDayOfWeek(DayOfWeek.Tuesday);

            var day = nthBusinessDay.AsOf(new DateTime(2016, 5, 30), 3);

            Assert.That(day, Is.EqualTo(new DateTime(2016, 6, 3)));
        }

        [Test]
        public void ShouldReturnsNthBusinessDayConsideringHolidays() {
            var nthBusinessDay = new NthBusinessDay {
                Holidays = new List<DateTime> {
                    new DateTime(2016, 5, 31)
                }
            };
            
            var day = nthBusinessDay.AsOf(new DateTime(2016, 5, 30), 3);

            Assert.That(day, Is.EqualTo(new DateTime(2016, 6, 3)));
        }

        [Test]
        public void ShouldReturnsNthBusinessDayConsideringIncludedSpecificDays() {
            var nthBusinessDay = new NthBusinessDay {
                DaysToInclude = new List<DateTime> {
                    new DateTime(2016, 6, 4)
                }
            };
            
            var day = nthBusinessDay.AsOf(new DateTime(2016, 5, 31), 5);

            Assert.That(day, Is.EqualTo(new DateTime(2016, 6, 6)));
        }

        [Test]
        public void IfNthBusinessIsNotInThePassedYearShouldBeThrowAnException() {
            var nthBusinessDay = new NthBusinessDay();

            Assert.Throws<BusinessDayOverlapsYearException>(() =>  nthBusinessDay.AsOf(new DateTime(2016, 12, 30), 3));
        }

        [Test]
        public void IfNthBusinessIsNotInThePassedYearMustSetRemainingDays() {
            try {
                var nthBusinessDay = new NthBusinessDay();

                nthBusinessDay.AsOf(new DateTime(2016, 12, 29), 3);
            } catch (BusinessDayOverlapsYearException e) {
                Assert.That(e.RemainingDays, Is.EqualTo(2));
            }
        }
    }
}
