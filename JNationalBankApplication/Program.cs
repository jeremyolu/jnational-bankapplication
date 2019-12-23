using JNationalBankApplication.Interfaces;
using JNationalBankApplication.Services;

namespace JNationalBankApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationStart(new MenuService(new CustomerService(), new AccountService(), new LoanService()));
        }

        static void ApplicationStart(IMenuService _menuService)
        {
            _menuService.DisplayApplicationMenu();
        }
    }
}
