using JNationalBankApplication.Interfaces;
using System;
using System.Data.Entity.Validation;
using System.Linq;

namespace JNationalBankApplication.Services
{
    public class LoanService : ILoanService
    {
        private readonly IDatabaseService _databaseService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;

        private const decimal minimumCustomerAge = 18;
        private const decimal minimumAccountBalance = 500.00M;

        private const decimal band1Rate = 12.6M;
        private const decimal band2Rate = 6.2M;
        private const decimal band3Rate = 3.4M;

        private const decimal LoanAdminRate = 75;

        public LoanService(IDatabaseService databaseService, ICustomerRepository customerRepository, IAccountRepository accountRepository)
        {
            _databaseService = databaseService;
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
        }

        public void ApplyCustomerLoan()
        {
            Console.Clear();
            Console.WriteLine("JNATIONAL CUSTOMER LOAN APPLICATION");
            Console.WriteLine();

            Console.WriteLine("ENTER CUSTOMER ACC NO: ");
            int accNo = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            var loan = new Loan();

            var customer = _customerRepository.ViewCustomerDetails(accNo);
            var customerAcc = _accountRepository.GetCustomerAccountDetails(accNo);

            var customerAge = customer.Age;
            var customerAccountBalance = customerAcc.Balance;

            var now = DateTime.Now;

            //implement unit test for this method
            if (CustomerLoanApplication(customerAge, customerAccountBalance))
            {
                Console.WriteLine("ENTER CUSTOMER LOAN AMOUNT: ");
                decimal loanAmount = Convert.ToDecimal(Console.ReadLine());

                try
                {
                    loan.AccNo = accNo;
                    loan.LoanAmount = loanAmount;
                    loan.LoanStartDate = now;
                    loan.LoanInterest = SetInterestLoanRate(customerAge);
                    loan.RepaymentDate = CalculateRepaymentDate(now, loanAmount);
                    loan.LoanInterestRepayment = CalculateInterestRepaymentAmount(SetInterestLoanRate(customerAge), loanAmount);
                    loan.FullRepaymentAmount = loan.LoanAmount + loan.LoanInterestRepayment;

                    _databaseService.Loans.Add(loan);
                    _databaseService.SaveDatabaseChanges();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine();
                    Console.WriteLine("CUSTOMER LOAN ACCEPTED AND LOAN ACCOUNT CREATED");
                    Console.ResetColor();
                }
                catch (DbEntityValidationException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR REGISTERING CUSTOMER ACCOUNT");
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
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("CUSTOMER DOES NOT QUALIFY FOR LOAN");
                Console.WriteLine();
                Console.WriteLine($"CUSTOMER IS NOT THE REQUIRED AGE: {minimumCustomerAge} OR CUSTOMER ACCOUNT BALANCE IS LESS THAN THE REQUIRED AMOUNT: {minimumAccountBalance}");
                Console.WriteLine();
                Console.WriteLine($"CUSTOMER ACC: {accNo} | CUSTOMER AGE: {customerAge} | CUSTOMER ACCOUNT BALANCE: {customerAccountBalance}");
                Console.ResetColor();
            }
        }

        public void ViewAllCustomerLoans()
        {
            Console.Clear();
            Console.WriteLine("JNATIONAL CUSTOMER LOANS");

            var loans = _databaseService.Loans.ToList();

            foreach (var loan in loans)
            {
                Console.WriteLine("---------------------------------");
                Console.WriteLine($"LOAN ID: {loan.LoanNo}");
                Console.WriteLine($"LOAN ACC NO: {loan.AccNo}");
                Console.WriteLine($"LOAN AMOUNT: {loan.LoanAmount}");
                Console.WriteLine($"LOAN INTEREST: {loan.LoanInterest}");
                Console.WriteLine($"INTEREST REPAYMENT: {loan.LoanInterestRepayment}");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"FULL PAYMENT AMOUNT: {loan.FullRepaymentAmount}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"LOAN START DATE: {loan.LoanStartDate}");
                Console.WriteLine($"LOAN REPAYMENT DATE: {loan.RepaymentDate}");
            }

            Console.WriteLine();
            Console.WriteLine("PRESS ENTER TO CONTINUE");
        }

        //implement unit test for this method
        public bool CustomerLoanApplication(int customerAge, decimal customerAccountBalance)
        {
            if (customerAge < minimumCustomerAge || customerAccountBalance < minimumAccountBalance)
            {
                return false;
            }

            return true;
        }

        //implement unit test for this method
        //loan method based on age, the younger, the higher the interest rate on loan
        public decimal SetInterestLoanRate(int customerAge)
        {
            if (customerAge >= 18 && customerAge < 25)
            {
                return band1Rate;
            }
            else if (customerAge >= 25 && customerAge < 45)
            {
                return band2Rate;
            }

            return band3Rate;
        }

        //implement unit test for this method
        public DateTime CalculateRepaymentDate(DateTime loanStartDate, decimal loanAmount)
        {
            if(loanAmount >= 500 && loanAmount < 1000)
            {
                return loanStartDate.AddMonths(9); ; 
            }
            else if(loanAmount >= 1000 && loanAmount < 3000)
            {
                return loanStartDate.AddMonths(18); ;
            }
            else if(loanAmount >= 3000 && loanAmount < 5000)
            {
                return loanStartDate.AddMonths(27);
            }

            return loanStartDate.AddMonths(32);
        }

        //implement unit test for this method
        public decimal CalculateInterestRepaymentAmount(decimal interestLoan, decimal loanAmount)
        {
            return (interestLoan * loanAmount) / 100 + LoanAdminRate;
        }
    }
}
