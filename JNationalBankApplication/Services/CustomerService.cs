using JNationalBankApplication.Interfaces;
using System;
using System.Data.Entity.Validation;

namespace JNationalBankApplication.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IDatabaseService _databaseService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IConsoleHelpher _consoleHelpher;

        private const string sortCode = "30-14-68";

        public CustomerService(IDatabaseService databaseService, ICustomerRepository customerRepository, 
            IAccountRepository accountRepository, IConsoleHelpher consoleHelpher)
        {
            _databaseService = databaseService;
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _consoleHelpher = consoleHelpher;
        }

        public void RegisterCustomerAccount()
        {
            _consoleHelpher.ClearScreen();
            _consoleHelpher.DisplayText("REGISTER NEW CUSTOMER ACCOUNT:");
            _consoleHelpher.DisplayText("-------------------------------");

            var customer = new Customer();
            var account = new Account();

            int accNo = GenerateNewAccountNo();

            _consoleHelpher.DisplayText("ENTER NAME");
            string name = _consoleHelpher.GetUserInput();
            _consoleHelpher.TextFormatLine();

            _consoleHelpher.DisplayText("ENTER SURNAME");
            string surname = _consoleHelpher.GetUserInput();
            _consoleHelpher.TextFormatLine();

            _consoleHelpher.DisplayText("ENTER AGE");
            int age = Convert.ToInt32(Console.ReadLine());
            _consoleHelpher.TextFormatLine();

            _consoleHelpher.DisplayText("ENTER POSTCODE");
            string postcode = _consoleHelpher.GetUserInput();
            _consoleHelpher.TextFormatLine();

            try
            {
                customer.Name = name;
                customer.Surname = surname;
                customer.Age = age;
                customer.PostCode = postcode;
                customer.AccountNo = accNo;

                account.AccountNo = accNo;
                account.SortCode = sortCode;

                _databaseService.Customers.Add(customer);
                _databaseService.Accounts.Add(account);
                _databaseService.SaveDatabaseChanges();

                _consoleHelpher.SetTextColour("GREEN");
                _consoleHelpher.DisplayText($"CUSTOMER ACCOUNT SUCCESFULLY REGISTERED - ACCOUNT NO: {accNo}");
                _consoleHelpher.ResetColour();
            }
            catch (DbEntityValidationException e)
            {
                _consoleHelpher.SetTextColour("GREEN");
                _consoleHelpher.DisplayText("ERROR REGISTERING CUSTOMER ACCOUNT");
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                _consoleHelpher.ResetColour();
                throw;
                    
            }
        }

        public void ViewCustomersAccounts()
        {
            _consoleHelpher.ClearScreen();
            _consoleHelpher.DisplayText("CUSTOMER & ACCOUNTS");
            _consoleHelpher.DisplayText("-------------------");

            DisplayCustomerAccountsMenu();
        }

        private int GenerateNewAccountNo()
        {
            var generatedAccNo = new Random();

            return generatedAccNo.Next(1000, 9999);
        }

        public void DisplayCustomerAccountsMenu()
        {
            _consoleHelpher.DisplayText("press 1 to view all customers");
            _consoleHelpher.DisplayText("press 2 to view all accounts");
            _consoleHelpher.DisplayText("press 3 to view a customer account");
            _consoleHelpher.TextFormatLine();
            _consoleHelpher.DisplayText("MENU OPTION:");
            int input = Convert.ToInt32(Console.ReadLine());
            
            switch(input)
            {
                case 1:
                    ViewAllCustomers();
                    break;
                case 2:
                    ViewAllAccounts();
                    break;
                case 3:
                    ViewCustomerAccountDetails();
                    break;
                default:
                    _consoleHelpher.DisplayText("incorrect menu option - navigating to main menu");
                    _consoleHelpher.TextFormatLine();
                    break;
            }
        }

        private void ViewAllCustomers()
        {
            _consoleHelpher.TextFormatLine();
            _consoleHelpher.DisplayText("JNATIONAL BANK CUSTOMERS");

            var customers = _customerRepository.ViewAllCustomers();

            foreach (var customer in customers)
            {
                _consoleHelpher.DisplayText("---------------------------------");
                _consoleHelpher.DisplayText($"NAME: {customer.Name.ToUpper()}");
                _consoleHelpher.DisplayText($"SURNAME: {customer.Surname.ToUpper()}");
                _consoleHelpher.DisplayText($"AGE: {customer.Age}");
                _consoleHelpher.DisplayText($"POSTCODE: {customer.PostCode.ToUpper()}");
                _consoleHelpher.DisplayText($"ACCOUNT NO: {customer.AccountNo}");
            }

            _consoleHelpher.TextFormatLine();
            _consoleHelpher.DisplayText("PRESS ENTER TO CONTINUE");
        }

        private void ViewAllAccounts()
        {
            _consoleHelpher.ClearScreen();
            _consoleHelpher.DisplayText("JNATIONAL BANK CURRENT ACCOUNTS");

            var accounts = _accountRepository.GetAllCustomerAccounts();

            foreach (var account in accounts)
            {
                _consoleHelpher.DisplayText("---------------------------------");
                _consoleHelpher.DisplayText($"ACCOUNT NO: {account.AccountNo}");
                _consoleHelpher.DisplayText($"SORT CODE: {account.SortCode}");
                _consoleHelpher.DisplayText($"AVAILABLE BALANCE: {account.Balance.ToString("C")}");
            }

            _consoleHelpher.TextFormatLine();
            _consoleHelpher.DisplayText("PRESS ENTER TO CONTINUE");
        }

        private void ViewCustomerAccountDetails()
        {
            _consoleHelpher.ClearScreen();
            _consoleHelpher.DisplayText("JNATIONAL CUSTOMER ACCOUNT DETAILS");
            _consoleHelpher.TextFormatLine();
            _consoleHelpher.DisplayText("ENTER CUSTOMER ACCOUNT: ");
            int accNo = Convert.ToInt32(Console.ReadLine());

            if (accNo == null)
            {
                _consoleHelpher.DisplayText("no account number has been inserted");
                _consoleHelpher.DisplayText("do you want to search customer with postcode instead?");
                return;
            }

            //implement join query to join customer and account table details in one query when accNo is found...

            var customer = _customerRepository.ViewCustomerDetails(accNo);
            var account = _accountRepository.GetCustomerAccountDetails(accNo);
            var accountBalance = account.Balance;

            _consoleHelpher.DisplayText("---------------------------------");
            _consoleHelpher.DisplayText($"CUSTOMER ID: {customer.CustomerID}");
            _consoleHelpher.DisplayText($"NAME: {customer.Name.ToUpper()}");
            _consoleHelpher.DisplayText($"SURNAME: {customer.Surname.ToUpper()}");
            _consoleHelpher.DisplayText($"AGE: {customer.Age}");
            _consoleHelpher.DisplayText($"POSTCODE: {customer.PostCode.ToUpper()}");
            _consoleHelpher.DisplayText($"ACCOUNT NO: {customer.AccountNo}");
            _consoleHelpher.DisplayText($"SORTCODE: {account.SortCode}");
            _consoleHelpher.DisplayText($"BALANCE : {account.Balance}");
            _consoleHelpher.DisplayText("---------------------------------");

            DisplayBalanceStatus(accountBalance);
        }

        //implement unit test for method
        public void DisplayBalanceStatus(decimal balance)
        {
            if(balance < 0)
            {
                _consoleHelpher.SetTextColour("RED");
                _consoleHelpher.DisplayText("WARNING: customer current account balance is negative");
                _consoleHelpher.ResetColour();
            }
            else
            {
                _consoleHelpher.SetTextColour("GREEN");
                _consoleHelpher.DisplayText("INFO: customer current account balance is normal");
                _consoleHelpher.ResetColour();
            }
        }
    }
}
