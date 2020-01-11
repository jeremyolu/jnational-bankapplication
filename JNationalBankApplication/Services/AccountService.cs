using JNationalBankApplication.Interfaces;
using System;
using System.Linq;

namespace JNationalBankApplication.Services
{
    public class AccountService : IAccountService
    {
        private readonly IDatabaseService _databaseService;
        private readonly IAccountRepository _accountRepository;
        private readonly IConsoleHelpher _consoleHelpher;

        public AccountService(IDatabaseService databaseService, IAccountRepository accountRepository, IConsoleHelpher consoleHelper)
        {
            _databaseService = databaseService;
            _accountRepository = accountRepository;
            _consoleHelpher = consoleHelper;
        }

        public void DepositBalance()
        {
            _consoleHelpher.ClearScreen();
            _consoleHelpher.DisplayText("JNATIONAL CUSTOMER DEPOSIT");

            _consoleHelpher.DisplayText("ENTER CUSTOMER ACC NO: ");
            int accNo = Convert.ToInt32(Console.ReadLine());

            _consoleHelpher.DisplayText("ENTER DEPOSIT AMOUNT: ");
            decimal amount = Convert.ToDecimal(Console.ReadLine());

            var account = _accountRepository.GetCustomerAccountDetails(accNo);

            decimal currentBalance = account.Balance;

            decimal newBalance = DepositAmount(currentBalance, amount);

            try
            {
                account.Balance = newBalance;
                _databaseService.SaveDatabaseChanges();
                _consoleHelpher.TextFormatLine();
                _consoleHelpher.SetTextColour("GREEN");
                _consoleHelpher.DisplayText($"{amount.ToString("C")} successfully deposited");
                _consoleHelpher.TextFormatLine();
                _consoleHelpher.DisplayText($"AVAILABLE BALANCE: {newBalance.ToString("C")}");
                _consoleHelpher.TextFormatLine();
            }
            catch(Exception ex)
            {
                _consoleHelpher.SetTextColour("RED");
                _consoleHelpher.ResetColour();
            }
        }

        public void WithdrawBalance()
        {
            _consoleHelpher.ClearScreen();
            _consoleHelpher.DisplayText("JNATIONAL WITHDRAW BALANCE");

            _consoleHelpher.DisplayText("ENTER CUSTOMER ACC NO: ");
            int accNo = Convert.ToInt32(Console.ReadLine());

            _consoleHelpher.DisplayText("ENTER WITHDRAWN AMOUNT: ");
            decimal withdrawAmount = Convert.ToDecimal(Console.ReadLine());

            var account = _accountRepository.GetCustomerAccountDetails(accNo);

            decimal currentBalance = account.Balance;

            decimal newBalance = WithdrawAmount(currentBalance, withdrawAmount);

            try
            {
                account.Balance = newBalance;
                _databaseService.SaveDatabaseChanges();
                _consoleHelpher.ClearScreen();
                _consoleHelpher.SetTextColour("GREEN");
                _consoleHelpher.DisplayText($"{withdrawAmount.ToString("C")} successfully withdrawn");
                _consoleHelpher.TextFormatLine();
                _consoleHelpher.DisplayText($"AVAILABLE BALANCE: {newBalance.ToString("C")}");
                _consoleHelpher.ResetColour();
            }
            catch (Exception ex)
            {
                _consoleHelpher.SetTextColour("GREEN");
                _consoleHelpher.ResetColour();
            }
        }

        public void SendCustomerPayment()
        {
            _consoleHelpher.ClearScreen();
            _consoleHelpher.DisplayText("JNATIONAL CUSTOMER PAYMEMT");
            _consoleHelpher.ClearScreen();

            _consoleHelpher.DisplayText("ENTER CUSTOMER ACC NO: ");
            int accNo = Convert.ToInt32(Console.ReadLine());
            _consoleHelpher.ClearScreen();

            _consoleHelpher.DisplayText("ENTER PAYEE ACC NO: ");
            int paymentAccNo = Convert.ToInt32(Console.ReadLine());
            _consoleHelpher.ClearScreen();

            _consoleHelpher.DisplayText("ENTER PAYMENT AMOUNT: ");
            decimal paymentAmount = Convert.ToDecimal(Console.ReadLine());
            _consoleHelpher.ClearScreen();

            var customerAcc = _accountRepository.GetCustomerAccountDetails(accNo);
            var payeeAcc = _accountRepository.GetCustomerAccountDetails(paymentAccNo);

            decimal customerBalance = customerAcc.Balance;
            decimal payeeBalance = payeeAcc.Balance;

            decimal updatedCustomerBalance = CheckUpdateCustomerBalance(customerBalance, paymentAmount);
            decimal updatedPayeeBalance = CheckUpdatePayeeBalance(payeeBalance, paymentAmount);

            try
            {
                customerAcc.Balance = updatedCustomerBalance;
                payeeAcc.Balance = updatedPayeeBalance;
                _databaseService.SaveDatabaseChanges();

                _consoleHelpher.ClearScreen();
                _consoleHelpher.TextFormatLine();
                _consoleHelpher.DisplayText($"{paymentAmount.ToString("C")} successfully sent to acc no: {paymentAccNo}");
                _consoleHelpher.TextFormatLine();
                _consoleHelpher.DisplayText($"CUSTOMER BALANCE: {updatedCustomerBalance.ToString("C")}");
                _consoleHelpher.TextFormatLine();
                _consoleHelpher.DisplayText($"PAYEE BALANCE: {updatedPayeeBalance.ToString("C")}");
                _consoleHelpher.ResetColour();
            }
            catch(Exception ex)
            {

            }

        }

        //implement unit test for this method
        public decimal DepositAmount(decimal balance, decimal amount)
        {
            if (amount <= 0)
            {
                _consoleHelpher.DisplayText("cannot deposit less than £0.00 in a transaction");
                // throw exception
                throw new ArgumentOutOfRangeException("amount", "amount cannot be less than £0.00 in a transaction");
            }
            else if (amount >= 1000)
            {
                _consoleHelpher.DisplayText("cannot deposit more than £1000.00 of cash in a transaction");
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
                _consoleHelpher.DisplayText("cannot withdraw less than £0.00");
                // throw exception
                throw new ArgumentOutOfRangeException("WithdrawAmount", "withdraw amount cannot be less than £0.00");
            }
            else if(withdrawAmount > balance)
            {
                _consoleHelpher.DisplayText("cannot withdraw more than account balance");
                // throw exception
                throw new ArgumentOutOfRangeException("WithdrawAmount", "cannot withdraw more than account balance");
            }

            return balance - withdrawAmount;
        }

        //implement unit test for this method
        public decimal CheckUpdateCustomerBalance(decimal customerBalance, decimal paymentAmount)
        {
            if (customerBalance < paymentAmount)
            {
                _consoleHelpher.DisplayText("cannot send this amount because customer account balance has insufficient funds");
                throw new ArgumentOutOfRangeException();
            }

            return customerBalance -= paymentAmount;
        }

        //implement unit test for this method
        public decimal CheckUpdatePayeeBalance(decimal payeeBalance, decimal paymentAmount)
        {
            return payeeBalance += paymentAmount;
        }
    }
}
