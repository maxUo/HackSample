using System;
using System.Windows.Input;

namespace UnblockHackNET.BL.ViewModels.MainBurger
{
    public class MenuViewModel : BaseViewModel
    {
        public ICommand GoToSettingsCommand => MakeMenuCommand(Pages.Settings);
        public ICommand GoToMainCommand => MakeMenuCommand(Pages.Main);

        private static ICommand MakeMenuCommand(object page)
        {
            return MakeNavigateToCommand(page, NavigationMode.RootPage, newNavigationStack: true, withAnimation: true);
        }
    }
}
