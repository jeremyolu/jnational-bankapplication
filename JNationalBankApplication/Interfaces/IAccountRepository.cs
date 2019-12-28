using System.Collections.Generic;

namespace JNationalBankApplication.Interfaces
{
    public interface IAccountRepository
    {
        Account GetCustomerAccountDetails(int accNo);

        List<Account> GetAllCustomerAccounts();
    }
}
