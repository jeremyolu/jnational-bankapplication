using Autofac;
using JNationalBankApplication.Data;
using JNationalBankApplication.Interfaces;
using JNationalBankApplication.Repositories;
using JNationalBankApplication.Services;

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
            builder.RegisterType<CustomerService>().As<ICustomerService>();
            builder.RegisterType<AccountService>().As<IAccountService>();
            builder.RegisterType<LoanService>().As<ILoanService>();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>();
            builder.RegisterType<AccountRepository>().As<IAccountRepository>();

            return builder.Build();
        }
    }
}
