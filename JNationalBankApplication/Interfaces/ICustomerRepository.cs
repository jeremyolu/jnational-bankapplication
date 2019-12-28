using System.Collections.Generic;

namespace JNationalBankApplication.Interfaces
{
    public interface ICustomerRepository
    {
        Customer ViewCustomerDetails(int accNo);

        List<Customer> ViewAllCustomers();
    }
}
