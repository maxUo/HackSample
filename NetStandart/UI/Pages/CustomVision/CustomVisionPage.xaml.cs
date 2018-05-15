using System;
using System.Collections.Generic;
using UnblockHackNET.BL.ViewModels.CustomVision;
using Xamarin.Forms;

namespace UnblockHackNET.UI.Pages.CustomVision
{
	public partial class CustomVisionPage : BasePage
	{
		public CustomVisionPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			var vm = BindingContext as CustomVisionViewModel;
			if (vm != null)
			{
				vm.OnApperCommand?.Execute(null);
			}
		}
	}
}
