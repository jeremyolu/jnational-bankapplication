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
        private readonly IConsoleHelpher _consoleHelper;

        private const decimal minimumCustomerAge = 18;
        private const decimal minimumAccountBalance = 500.00M;

        private const decimal band1Rate = 12.6M;
        private const decimal band2Rate = 6.2M;
        private const decimal band3Rate = 3.4M;

        private const decimal LoanAdminRate = 75;

        public LoanService(IDatabaseService databaseService, ICustomerRepository customerRepository, 
            IConsoleHelpher consoleHelper, IAccountRepository accountRepository)
        {
            _databaseService = databaseService;
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _consoleHelper = consoleHelper;
        }

        public void ApplyCustomerLoan()
        {
            _consoleHelper.ClearScreen();
            _consoleHelper.DisplayText("JNATIONAL CUSTOMER LOAN APPLICATION");
            _consoleHelper.TextFormatLine();

            _consoleHelper.DisplayText("ENTER CUSTOMER ACC NO: ");
            int accNo = Convert.ToInt32(Console.ReadLine());
            _consoleHelper.TextFormatLine();

            var loan = new Loan();

            var customer = _customerRepository.ViewCustomerDetails(accNo);
            var customerAcc = _accountRepository.GetCustomerAccountDetails(accNo);

            var customerAge = customer.Age;
            var customerAccountBalance = customerAcc.Balance;

            var now = DateTime.Now;

            //implement unit test for this method
            if (CustomerLoanApplication(customerAge, customerAccountBalance))
            {
                _consoleHelper.DisplayText("ENTER CUSTOMER LOAN AMOUNT: ");
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

                    _consoleHelper.SetTextColour("GREEN")
;                   _consoleHelper.TextFormatLine();
                    _consoleHelper.DisplayText("CUSTOMER LOAN ACCEPTED AND LOAN ACCOUNT CREATED");
                    _consoleHelper.ResetColour();
                }
                catch (DbEntityValidationException e)
                {
                    _consoleHelper.SetTextColour("RED");
                    _consoleHelper.DisplayText("ERROR REGISTERING CUSTOMER ACCOUNT");
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    _consoleHelper.ResetColour();
                    throw;
                }
            }
            else
            {
                _consoleHelper.SetTextColour("RED");
                _consoleHelper.DisplayText("CUSTOMER DOES NOT QUALIFY FOR LOAN");
                _consoleHelper.TextFormatLine();
                _consoleHelper.DisplayText($"CUSTOMER IS NOT THE REQUIRED AGE: {minimumCustomerAge} OR CUSTOMER ACCOUNT BALANCE IS LESS THAN THE REQUIRED AMOUNT: {minimumAccountBalance}");
                _consoleHelper.TextFormatLine();
                _consoleHelper.DisplayText($"CUSTOMER ACC: {accNo} | CUSTOMER AGE: {customerAge} | CUSTOMER ACCOUNT BALANCE: {customerAccountBalance}");
                _consoleHelper.ResetColour();
            }
        }

        public void ViewAllCustomerLoans()
        {
            _consoleHelper.ClearScreen();
            _consoleHelper.DisplayText("JNATIONAL CUSTOMER LOANS");

            var loans = _databaseService.Loans.ToList();

            foreach (var loan in loans)
            {
                _consoleHelper.DisplayText("---------------------------------");
                _consoleHelper.DisplayText($"LOAN ID: {loan.LoanNo}");
                _consoleHelper.DisplayText($"LOAN ACC NO: {loan.AccNo}");
                _consoleHelper.DisplayText($"LOAN AMOUNT: {loan.LoanAmount}");
                _consoleHelper.DisplayText($"LOAN INTEREST: {loan.LoanInterest}");
                _consoleHelper.DisplayText($"INTEREST REPAYMENT: {loan.LoanInterestRepayment}");
                _consoleHelper.SetTextColour("RED");
                _consoleHelper.DisplayText($"FULL PAYMENT AMOUNT: {loan.FullRepaymentAmount}");
                _consoleHelper.SetTextColour("WHITE");
                _consoleHelper.DisplayText($"LOAN START DATE: {loan.LoanStartDate}");
                _consoleHelper.DisplayText($"LOAN REPAYMENT DATE: {loan.RepaymentDate}");
                _consoleHelper.DisplayText("---------------------------------");
            }

            _consoleHelper.TextFormatLine();
            _consoleHelper.DisplayText("PRESS ENTER TO CONTINUE");
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
