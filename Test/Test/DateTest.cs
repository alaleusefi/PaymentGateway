using System;
using Xunit;
using Core;
using Moq;

namespace Test
{
    public class DateTest
    {
        private const int MIN_YEAR = 1982;
        private const int MIN_MONTH = 1;
        private const int MAX_MONTH = 12;

        [Fact]
        public void Create_InitialisesState()
        {
            var instance = Date.Create(2022, 3);
            Assert.Equal(2022, instance.Year);
            Assert.Equal(3, instance.Month);
        }

        [Fact]
        public void IsValid_IsFalse_IfYearBelowRange()
        {
            var instance = Date.Create(MIN_YEAR - 1, 3);
            Assert.False(instance.IsValid);
        }

        [Fact]
        public void IsValid_ChecksLowerBoundOfMonth()
        {
            var instance = Date.Create(DateTime.Now.Year, MIN_MONTH - 1);
            Assert.False(instance.IsValid);
        }

        [Fact]
        public void IsValid_ChecksUpperBoundOfMonth()
        {
            var instance = Date.Create(DateTime.Now.Year, MAX_MONTH + 1);
            Assert.False(instance.IsValid);
        }

        [Fact]
        public void IsPassed_IsTrueForPast()
        {
            var lastMonth = DateTime.Now.AddMonths(-1);
            var instance = Date.Create(lastMonth.Year, lastMonth.Month);
            Assert.True(instance.IsPassed);
        }

        [Fact]
        public void IsPassed_IsFalseForCurrentMonth()
        {
            var now = DateTime.Now;
            var instance = Date.Create(now.Year, now.Month);
            Assert.False(instance.IsPassed);
        }

        [Fact]
        public void IsPassed_IsFalseForFuture()
        {
            var nextMonth = DateTime.Now.AddMonths(+1);
            var instance = Date.Create(nextMonth.Year, nextMonth.Month);
            Assert.False(instance.IsPassed);
        }
    }
}
