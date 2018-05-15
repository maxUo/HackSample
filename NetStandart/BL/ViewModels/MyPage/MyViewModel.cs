using System;
using System.Collections.ObjectModel;
using UnblockHackNET.UI.Pages.Views;

namespace UnblockHackNET.BL.ViewModels.MyPage
{
	public class MyViewModel : BaseViewModel
	{
		public ObservableCollection<MyView> ViewItemSource
		{
			get => Get(new ObservableCollection<MyView>());
			set => Set(value);
		}

		public MyViewModel()
		{
			
			ViewItemSource = new ObservableCollection<MyView>();
			ViewItemSource.Add(new MyView(
				new ObservableCollection<SampleTextClass>(){
				new SampleTextClass{
					Header="Сфотографируйте льва",
					Description="Вам нужно найти льва и сфотографировать."
				}
			}));
			ViewItemSource.Add(new MyView(
                new ObservableCollection<SampleTextClass>(){
                new SampleTextClass{
                    Header="Сфотографируйте Жирафа",
                    Description="Вам нужно найти жирафа и сфотографировать."
                }
            }));

			OnPropertyChanged(nameof(ViewItemSource));
		}
		private static Random random = new Random();
		private MyView GenerateView()
		{
			var t = new ObservableCollection<SampleTextClass>();
			for (int j = 0; j < random.Next(25, 50); j++)
			{
				t.Add(new SampleTextClass
				{
					Header = $"RosumHeader{j}",
					Description = $"RosumDescription{j}"
				});
			}
			return new MyView(t);
		}
	}
}
