using JNationalBankApplication.Interfaces;
using System;

namespace JNationalBankApplication.Services
{
    public class MenuService : IMenuService
    {
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        private readonly ILoanService _loanService;
        private readonly IConsoleHelpher _consoleHelper;

        public MenuService(ICustomerService customerService, IAccountService accountService, 
            ILoanService loanService, IConsoleHelpher consoleHelper)
        {
            _customerService = customerService;
            _accountService = accountService;
            _loanService = loanService;
            _consoleHelper = consoleHelper;
        }

        public void DisplayApplicationMenu()
        {
            _consoleHelper.SetTextColour("WHITE");
            _consoleHelper.DisplayText("JNational Bank Application v.1.0.0");
            _consoleHelper.DisplayText("----------------------------------");

            int menuSelection = -1;

            while(menuSelection != 0)
            {
                _consoleHelper.DisplayText("press 1 to register a customer account");
                _consoleHelper.DisplayText("press 2 to view customers and accounts");
                _consoleHelper.DisplayText("press 3 to deposit amount");
                _consoleHelper.DisplayText("press 4 to withdraw amount");
                _consoleHelper.DisplayText("press 5 to send customer payment");
                _consoleHelper.DisplayText("press 6 to apply for customer loan");
                _consoleHelper.DisplayText("press 7 to view all customer loans");
                _consoleHelper.DisplayText("press 0 to exit");
                _consoleHelper.DisplayText("----------------------------------");

                _consoleHelper.DisplayText("ENTER MENU SELECTION:");
                var input = Console.ReadLine();

                if(int.TryParse(input, out menuSelection))
                {
                    switch (menuSelection)
                    {
                        case 0:
                            ExitApplication();
                            break;
                        case 1:
                            _customerService.RegisterCustomerAccount();
                            break;
                        case 2:
                            _customerService.ViewCustomersAccounts();
                            break;
                        case 3:
                            _accountService.DepositBalance();
                            break;
                        case 4:
                            _accountService.WithdrawBalance();
                            break;
                        case 5:
                            _accountService.SendCustomerPayment();
                            break;
                        case 6:
                            _loanService.ApplyCustomerLoan();
                            break;
                        case 7:
                            _loanService.ViewAllCustomerLoans();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    _consoleHelper.DisplayText("insert a valid menu input selection");
                }
                Console.ReadLine();
            }
        }

        public void ExitApplication()
        {
            _consoleHelper.DisplayText("APPLICATION EXIT -");
            Environment.Exit(0);
        }
    }
}
