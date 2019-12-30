using Autofac;
using JNationalBankApplication.Data;
using JNationalBankApplication.Interfaces;
using JNationalBankApplication.Repositories;
using JNationalBankApplication.Services;
using JNationalBankApplication.Utilities;

namespace JNationalBankApplication
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Application>().As<IApplication>();
            builder.RegisterType<MenuService>().As<IMenuService>();

            builder.RegisterType<DatabaseService>().As<IDatabaseService>();
            builder.RegisterType<ConsoleHelper>().As<IConsoleHelpher>();
            builder.RegisterType<InputValidation>().As<IInputValidation>();
            builder.RegisterType<CustomerService>().As<ICustomerService>();
            builder.RegisterType<AccountService>().As<IAccountService>();
            builder.RegisterType<LoanService>().As<ILoanService>();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>();
            builder.RegisterType<AccountRepository>().As<IAccountRepository>();

            return builder.Build();
        }
    }
}
