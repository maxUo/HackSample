using System;
using System.Windows.Input;

namespace UnblockHackNET.BL.ViewModels.MainBurger
{
    public class MainViewModel : BaseViewModel
    {
		public ICommand GoToMyPage => MakeNavigateToCommand(Pages.My,
															NavigationMode.Normal,
															withAnimation: true,
															newNavigationStack: false);
		/*
        public ICommand GoToHistoryCommand => MakeNavigateToCommand(Pages.VotingHistory,
                                                                    NavigationMode.Normal, 
                                                                    withAnimation: true,
                                                                    newNavigationStack: false);
        
        public ICommand GoToCurVotingCommand => MakeNavigateToCommand(Pages.CurrentVotings,
                                                                    NavigationMode.Normal,
                                                                    withAnimation: true,
                                                                    newNavigationStack: false);
                                                                    */

    }
}
