using Autofac;
using JNationalBankApplication.Interfaces;

namespace JNationalBankApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var application = scope.Resolve<IApplication>();

                application.ApplicationStart();
            }
        }
    }
}
