using System;

namespace JNationalBankApplication.Interfaces
{
    public interface IDateHelper
    {
        DateTime GetCurrentDateTime();

        DateTime SetDays(DateTime date, int days);

        DateTime SetMonths(DateTime date, int days);

        DateTime SetYears(DateTime date, int days);
    }
}
