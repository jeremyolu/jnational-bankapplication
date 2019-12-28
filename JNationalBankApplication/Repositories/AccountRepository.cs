using JNationalBankApplication.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace JNationalBankApplication.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDatabaseService _databaseService;

        public AccountRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public Account GetCustomerAccountDetails(int accNo)
        {
            return _databaseService.Accounts.Where(a => a.AccountNo == accNo).FirstOrDefault();
        }

        public List<Account> GetAllCustomerAccounts()
        {
            return _databaseService.Accounts.ToList();
        }
    }
}
