using System.Data.Entity;

namespace JNationalBankApplication.Interfaces
{
    public interface IDatabaseService
    {
        IDbSet<Customer> Customers { get; set; }
        IDbSet<Account> Accounts { get; set; }
        IDbSet<Loan> Loans { get; set; }

        void SaveDatabaseChanges();
        void DisposeResources(bool disposing);
    }
}