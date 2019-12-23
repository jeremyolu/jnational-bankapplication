using JNationalBankApplication.Interfaces;
using System;
using System.Linq;

namespace JNationalBankApplication.Services
{
    public class LoanService : ILoanService
    {
        JNationalBankDbContext _context = new JNationalBankDbContext();

        private const decimal minimumCustomerAge = 18;
        private const decimal minimumAccountBalance = 500.00M;

        private const decimal band1Rate = 12.6M;
        private const decimal band2Rate = 6.2M;
        private const decimal band3Rate = 3.4M;

        public void ApplyCustomerLoan()
        {
            Console.Clear();
            Console.WriteLine("JNATIONAL CUSTOMER LOAN APPLICATION");
            Console.WriteLine();

            Console.WriteLine("ENTER CUSTOMER ACC NO: ");
            int accNo = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            var loan = new Loan();

            var customer = _context.Customers.Where(c => c.AccountNo == accNo).FirstOrDefault();
            var customerAcc = _context.Accounts.Where(c => c.AccountNo == accNo).FirstOrDefault();

            var customerAge = customer.Age;
            var customerAccountBalance = customerAcc.Balance;

            var now = DateTime.Now;

            //implement unit test for this method
            //implement customer loan validation method - bool type?
            if (customerAge < minimumCustomerAge || customerAccountBalance < minimumAccountBalance)
            {
                throw new ArgumentOutOfRangeException();
            }

            Console.WriteLine("ENTER CUSTOMER LOAN AMOUNT: ");
            decimal loanAmount = Convert.ToInt32(Console.ReadLine());

            // if bool true
            loan.AccNo = accNo;
            loan.LoanAmount = loanAmount;
            loan.LoanStartDate = now;
            
            loan.LoanInterest = SetInterestLoanRate(customerAge);

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
        public DateTime CalculateDate(DateTime loanStartDate, decimal loanAmount)
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

    }
}
