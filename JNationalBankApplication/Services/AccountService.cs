using JNationalBankApplication.Interfaces;
using System;
using System.Linq;

namespace JNationalBankApplication.Services
{
    public class AccountService : IAccountService
    {
        JNationalBankDbContext _context = new JNationalBankDbContext();

        public void DepositBalance()
        {
            Console.Clear();
            Console.WriteLine("JNATIONAL CUSTOMER DEPOSIT");

            Console.WriteLine("ENTER CUSTOMER ACC NO: ");
            int accNo = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("ENTER DEPOSIT AMOUNT: ");
            decimal amount = Convert.ToDecimal(Console.ReadLine());

            var account = _context.Accounts.Where(c => c.AccountNo == accNo).FirstOrDefault();

            decimal currentBalance = account.Balance;

            decimal newBalance = DepositAmount(currentBalance, amount);

            try
            {
                account.Balance = newBalance;
                _context.SaveChanges();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{amount.ToString("C")} successfully deposited");
                Console.WriteLine();
                Console.WriteLine($"AVAILABLE BALANCE: {newBalance.ToString("C")}");
                Console.ResetColor();
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.ResetColor();
            }
        }

        public void WithdrawBalance()
        {
            Console.Clear();
            Console.WriteLine("JNATIONAL WITHDRAW BALANCE");

            Console.WriteLine("ENTER CUSTOMER ACC NO: ");
            int accNo = Convert.ToInt32(Console.ReadLine());

            /*
            Console.WriteLine("ENTER PAYEE ACC NO: ");
            int paymentAccNo = Convert.ToInt32(Console.ReadLine());
            */

            Console.WriteLine("ENTER WITHDRAWN AMOUNT: ");
            decimal withdrawAmount = Convert.ToDecimal(Console.ReadLine());

            var account = _context.Accounts.Where(c => c.AccountNo == accNo).FirstOrDefault();

            decimal currentBalance = account.Balance;

            decimal newBalance = WithdrawAmount(currentBalance, withdrawAmount);

            try
            {
                account.Balance = newBalance;
                _context.SaveChanges();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{withdrawAmount.ToString("C")} successfully withdrawn");
                Console.WriteLine();
                Console.WriteLine($"AVAILABLE BALANCE: {newBalance.ToString("C")}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.ResetColor();
            }
        }

        //implement unit test for this method
        public decimal DepositAmount(decimal balance, decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("cannot deposit less than £0.00 in a transaction");
                // throw exception
                throw new ArgumentOutOfRangeException("amount", "amount cannot be less than £0.00 in a transaction");
            }
            else if (amount >= 1000)
            {
                Console.WriteLine("cannot deposit more than £1000.00 of cash in a transaction");
                // throw exception
                throw new ArgumentOutOfRangeException("amount", "amount cannot be greater than £1000.00 in a transaction");
            }

            return balance + amount;
        }
        

        //implement unit test for this method
        public decimal WithdrawAmount(decimal balance, decimal withdrawAmount)
        {
            if (withdrawAmount <= 0)
            {
                Console.WriteLine("cannot withdraw less than £0.00");
                // throw exception
                throw new ArgumentOutOfRangeException("WithdrawAmount", "withdraw amount cannot be less than £0.00");
            }
            else if(withdrawAmount > balance)
            {
                Console.WriteLine("cannot withdraw more than account balance");
                // throw exception
                throw new ArgumentOutOfRangeException("WithdrawAmount", "cannot withdraw more than account balance");
            }

            return balance - withdrawAmount;
        }
    }
}
