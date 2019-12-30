using JNationalBankApplication.Interfaces;
using System;

namespace JNationalBankApplication.Utilities
{
    public class DateHelper : IDateHelper
    {
        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }

        public DateTime SetDays(DateTime date, int days)
        {
            return date.AddDays(days);
        }

        public DateTime SetMonths(DateTime date, int months)
        {
            return date.AddMonths(months);
        }

        public DateTime SetYears(DateTime date, int years)
        {
            return date.AddMonths(years);
        }
    }
}
