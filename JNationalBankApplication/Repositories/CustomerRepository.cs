using JNationalBankApplication.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace JNationalBankApplication.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDatabaseService _databaseService;

        public CustomerRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public Customer ViewCustomerDetails(int accNo)
        {
            return _databaseService.Customers.Where(c => c.AccountNo == accNo).FirstOrDefault();
        }

        public List<Customer> ViewAllCustomers()
        {
            return _databaseService.Customers.ToList();
        }

    }
}
