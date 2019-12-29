using JNationalBankApplication.Data;
using JNationalBankApplication.Interfaces;
using JNationalBankApplication.Repositories;
using JNationalBankApplication.Services;
using JNationalBankApplication.Utilities;
using Moq;
using NUnit.Framework;

namespace JNationalBankApplicationTests.CustomerServicesTests
{
    [TestFixture]
    public class CustomerTests
    {
        private CustomerService _customerService;
        [SetUp]
        public void SetUp()
        {
            _customerService = new CustomerService(new DatabaseService(), new CustomerRepository(new DatabaseService()),
                new AccountRepository(new DatabaseService()), new ConsoleHelper());
        }


        [Test]
        [TestCase(-1, "negative")]
        [TestCase(-50.52, "negative")]
        [TestCase(-150, "negative")]
        public void DisplayBalanceStatus_CustomerBalanceLessThan0_ReturnNegativeText(decimal balance, string expectedText)
        {
            //Act
            var result = _customerService.DisplayBalanceStatus(balance);

            //Assert
            Assert.That(result, Does.Contain(expectedText).IgnoreCase);
        }

        [Test]
        [TestCase(0, "normal")]
        [TestCase(231.33, "normal")]
        [TestCase(752.80, "normal")]
        public void DisplayBalanceStatus_CustomerBalanceGreaterThan0_ReturnNegativeText(decimal balance, string expectedText)
        {
            //Act
            var result = _customerService.DisplayBalanceStatus(balance);

            //Assert
            Assert.That(result, Does.Contain("normal").IgnoreCase);
        }
    }
}
