using JNationalBankApplication.Interfaces;
using System;

namespace JNationalBankApplication.Services
{
    public class MenuService : IMenuService
    {
        private readonly ICustomerService _customerService;

        public MenuService(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public void DisplayApplicationMenu()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("JNational Bank Application v.1.0.0");
            Console.WriteLine("----------------------------------");

            int menuSelection = -1;

            while(menuSelection != 0)
            {
                Console.WriteLine("press 1 to register a customer account");
                Console.WriteLine("press 2 to view customers and accounts");
                Console.WriteLine("press 3 to deposit amount");
                Console.WriteLine("press 4 to withdraw amount");
                Console.WriteLine("press 5 to apply for customer loan");
                Console.WriteLine("press 0 to exit");
                Console.WriteLine("----------------------------------");
                
                Console.WriteLine("ENTER MENU SELECTION:");
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
                        case 4:
                            break;
                        case 5:
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("insert a valid menu input selection");
                }
                Console.ReadLine();
            }
        }

        public void ExitApplication()
        {
            Console.WriteLine("APPLICATION EXIT -");
            Environment.Exit(0);
        }
    }
}
