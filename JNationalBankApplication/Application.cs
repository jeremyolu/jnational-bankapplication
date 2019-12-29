using JNationalBankApplication.Interfaces;

namespace JNationalBankApplication
{
    public class Application : IApplication
    {
        private readonly IMenuService _menuService;

        public Application(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public void ApplicationStart()
        {
            _menuService.DisplayApplicationMenu();
        }
    }
}
