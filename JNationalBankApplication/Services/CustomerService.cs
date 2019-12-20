using JNationalBankApplication.Interfaces;
using System;
using System.Data.Entity.Validation;
using System.Linq;

namespace JNationalBankApplication.Services
{
    public class CustomerService : ICustomerService
    {
        private const string sortCode = "30-14-68";

        public CustomerService()
        {

        }

        public void RegisterCustomerAccount()
        {
            Console.Clear();
            Console.WriteLine("REGISTER NEW CUSTOMER ACCOUNT -");
            Console.WriteLine("-------------------------------");

            var customer = new Customer();
            var account = new Account();

            int accNo = GenerateNewAccountNo();

            Console.WriteLine("ENTER NAME");
            string name = Console.ReadLine();

            Console.WriteLine("ENTER SURNAME");
            string surname = Console.ReadLine();

            Console.WriteLine("ENTER AGE");
            int age = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("ENTER POSTCODE");
            string postcode = Console.ReadLine();

            using (JNationalBankDbContext _context = new JNationalBankDbContext())
            {
                try
                {
                    customer.Name = name;
                    customer.Surname = surname;
                    customer.Age = age;
                    customer.PostCode = postcode;
                    customer.AccountNo = accNo;

                    account.AccountNo = accNo;
                    account.SortCode = sortCode;

                    _context.Customers.Add(customer);
                    _context.Accounts.Add(account);
                    _context.SaveChanges();

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"CUSTOMER ACCOUNT SUCCESFULLY REGISTERED - ACCOUNT NO: {accNo}");
                    Console.ResetColor();
                }
                catch (DbEntityValidationException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR REGISTERING CUSTOMER ACCOUNT ");
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    Console.ResetColor();
                    throw;
                    
                }
            }
        }

        public void ViewCustomersAccounts()
        {
            Console.Clear();
            Console.WriteLine("CUSTOMER & ACCOUNTS");
            Console.WriteLine("-------------------");

            DisplayCustomerAccountsMenu();
        }

        private int GenerateNewAccountNo()
        {
            int newAccountNo;
            var generatedAccNo = new Random();

            return newAccountNo = generatedAccNo.Next(1000, 9999);
        }

        public void DisplayCustomerAccountsMenu()
        {
            Console.WriteLine("press 1 to view all customers");
            Console.WriteLine("press 2 to view all accounts");
            Console.WriteLine("press 3 to view a customer account");
            Console.WriteLine();
            Console.WriteLine("MENU OPTION:");
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
                    Console.WriteLine("incorrect menu option - navigating to main menu");
                    Console.Clear();
                    break;
            }
        }

        private void ViewAllCustomers()
        {
            Console.Clear();
            Console.WriteLine("JNATIONAL BANK CUSTOMERS");

            using (JNationalBankDbContext _context = new JNationalBankDbContext())
            {
                var customers = _context.Customers.ToList();

                foreach (var customer in customers)
                {
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine($"NAME: {customer.Name.ToUpper()}");
                    Console.WriteLine($"SURNAME: {customer.Surname.ToUpper()}");
                    Console.WriteLine($"AGE: {customer.Age}");
                    Console.WriteLine($"POSTCODE: {customer.PostCode.ToUpper()}");
                    Console.WriteLine($"ACCOUNT NO: {customer.AccountNo}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("PRESS ENTER TO CONTINUE");
        }

        private void ViewAllAccounts()
        {
            Console.Clear();
            Console.WriteLine("JNATIONAL BANK CURRENT ACCOUNTS");

            using (JNationalBankDbContext _context = new JNationalBankDbContext())
            {
                var accounts = _context.Accounts.ToList();

                foreach (var account in accounts)
                {
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine($"ACCOUNT NO: {account.AccountNo}");
                    Console.WriteLine($"SORT CODE: {account.SortCode}");
                    Console.WriteLine($"AVAILABLE BALANCE: {account.Balance.ToString("C")}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("PRESS ENTER TO CONTINUE");
        }

        private void ViewCustomerAccountDetails()
        {
            JNationalBankDbContext _context = new JNationalBankDbContext();

            Console.Clear();
            Console.WriteLine("JNATIONAL CUSTOMER ACCOUNT DETAILS");
            Console.WriteLine();
            Console.WriteLine("ENTER CUSTOMER ACCOUNT: ");
            int? accNo = Convert.ToInt32(Console.ReadLine());

            if (accNo == null)
            {
                Console.WriteLine("no account number has been inserted");
                Console.WriteLine("do you want to search customer with postcode instead?");
                return;
            }
            
            //implement join query to join customer and account table details in one query when accNo is found...

            var customer = _context.Customers.Where(c => c.AccountNo == accNo).FirstOrDefault();
            var account = _context.Accounts.Where(c => c.AccountNo == accNo).FirstOrDefault();
            var accountBalance = account.Balance;

            Console.WriteLine("---------------------------------");
            Console.WriteLine($"CUSTOMER ID: {customer.CustomerID}");
            Console.WriteLine($"NAME: {customer.Name.ToUpper()}");
            Console.WriteLine($"SURNAME: {customer.Surname.ToUpper()}");
            Console.WriteLine($"AGE: {customer.Age}");
            Console.WriteLine($"POSTCODE: {customer.PostCode.ToUpper()}");
            Console.WriteLine($"ACCOUNT NO: {customer.AccountNo}");
            Console.WriteLine($"SORTCODE: {account.SortCode}");
            Console.WriteLine($"BALANCE : {account.Balance}");
            Console.WriteLine("---------------------------------");

            DisplayBalanceStatus(accountBalance);
        }

        //implement unit test for method
        public void DisplayBalanceStatus(decimal balance)
        {
            if(balance < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("WARNING: customer current account balance is negative");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("INFO: customer current account balance is normal");
                Console.ResetColor();
            }
        }
    }
}
