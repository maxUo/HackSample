using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UnblockHackNET.BL.ViewModels;
using Xamarin.Forms;

namespace UnblockHackNET.UI.Pages.Views
{
	public class SampleTextClass
	{
		public string Header { get; set; }
		public string Description { get; set; }
	}

	public partial class MyView : ContentView
	{
		public void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			if (sender is ListView)
			{
				var listView = sender as ListView;
				if (listView != null)
				{
					var t = (e.SelectedItem as SampleTextClass);
					if (t != null)
					{
						Vm.NavParam = new Dictionary<string, object>();
						Vm.NavParam.Add("Text", t.Description);
						Vm.GoToCustomVision?.Execute(null);
						listView.SelectedItem = null;
					}
				}
			}
		}
		private static MyViewViewModel Vm = new MyViewViewModel();

		public ObservableCollection<SampleTextClass> Musorka { get; set; }

		public MyView(ObservableCollection<SampleTextClass> tmp)
		{
			InitializeComponent();

			Musorka = tmp;
			OnPropertyChanged(nameof(Musorka));
			listV.ItemsSource = Musorka;
			BindingContext = Vm;

		}
	}

	public class MyViewViewModel : BaseViewModel
	{
		public ICommand GoToCustomVision => MakeNavigateToCommand(UnblockHackNET.Pages.CustomVision,
															NavigationMode.Normal,
															withAnimation: true,
															newNavigationStack: false,
																  navParams: NavParam);


		public Dictionary<string, object> NavParam = new Dictionary<string, object>();
	}
}
