using JNationalBankApplication.Interfaces;
using System.Data.Entity;

namespace JNationalBankApplication.Data
{
    public class DatabaseService : DbContext, IDatabaseService
    {
        public DatabaseService() : base("name=JNationalBankDbContext")
        {
        }

        public virtual IDbSet<Customer> Customers { get; set; }
        public virtual IDbSet<Account> Accounts { get; set; }
        public virtual IDbSet<Loan> Loans { get; set; }

        public void SaveDatabaseChanges()
        {
            SaveChanges();
        }

        public void DisposeResources(bool disposing)
        {
            if (disposing)
            {
                Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
