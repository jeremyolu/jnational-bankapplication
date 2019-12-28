using JNationalBankApplication.Data;
using JNationalBankApplication.Interfaces;
using JNationalBankApplication.Repositories;
using JNationalBankApplication.Services;

namespace JNationalBankApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationStart(new MenuService(new CustomerService(new DatabaseService(), 
                new CustomerRepository(new DatabaseService()), new AccountRepository(new DatabaseService())), 
                new AccountService(), new LoanService()));
        }

        static void ApplicationStart(IMenuService _menuService)
        {
            _menuService.DisplayApplicationMenu();
        }
    }
}
