using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Core
{
    public interface IPassable { bool IsPassed { get; } }

    public interface IDate : IPassable, IValidatable
    {
        int Year { get; }
        int Month { get; }
    }

    public class Date : IDate
    {
        private Date() { }
        private const int MIN_YEAR = 1982;
        private const int MIN_MONTH = 1;
        private const int MAX_MONTH = 12;
        public int Year { get; private set; }
        public int Month { get; private set; }
        public bool IsPassed
        {
            get
            {
                var now = DateTime.Now;
                if (Year < now.Year)
                    return true;
                if (Year > now.Year)
                    return false;
                return Month < now.Month;
            }
        }

        public static Date Create(int year, int month)
        {
            var instance = new Date();
            instance.Year = year;
            instance.Month = month;
            return instance;
        }

        public bool IsValid
        {
            get
            {
                if (Year < MIN_YEAR)
                    return false;
                if (Month < MIN_MONTH || Month > MAX_MONTH)
                    return false;
                return true;
            }
        }
    }
}