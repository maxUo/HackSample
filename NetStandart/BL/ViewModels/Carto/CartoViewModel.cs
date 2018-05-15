using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UnblockHackNET.BL.ViewModels.Carto
{
    public class HabitatData
    {
        public HabitatData(string postal, string animal)
        {
            Postal = postal;
            Animal = animal;
        }

        public string Postal { get; set; }

        public string Animal { get; set; }
    }

    public class CartoViewModel : BaseViewModel
    {
        public ObservableCollection<HabitatData> HabitatDataSource
        {
            get => Get(new ObservableCollection<HabitatData>());
            set => Set(value);
        }

        public ICommand OnAppear => MakeCommand(LoadValues);

        private async void LoadValues()
        {
            while(NavigationParams == null){
                await Task.Delay(50);
            }
            object text = "";
            NavigationParams.TryGetValue("AnimalLabel", out text);
            string label = text as string;

            HabitatDataSource = new ObservableCollection<HabitatData>();
            if (!string.IsNullOrWhiteSpace(label) && label == "Lion")
            {
                HabitatDataSource.Add(new HabitatData("RUS", "AnimalLabel"));
                HabitatDataSource.Add(new HabitatData("LY", "AnimalLabel"));
                HabitatDataSource.Add(new HabitatData("ML", "AnimalLabel"));
                HabitatDataSource.Add(new HabitatData("US", "AnimalLabel"));
                HabitatDataSource.Add(new HabitatData("BR", "AnimalLabel"));
                HabitatDataSource.Add(new HabitatData("AQ", "AnimalLabel"));
            }
            else if (!string.IsNullOrWhiteSpace(label) && label == "Coala")
            {
                HabitatDataSource = new ObservableCollection<HabitatData>();
                HabitatDataSource.Add(new HabitatData("US", "AnimalLabel"));
                HabitatDataSource.Add(new HabitatData("BR", "AnimalLabel"));
                HabitatDataSource.Add(new HabitatData("AQ", "AnimalLabel"));
            }
            else
            {
                //
            }

            OnPropertyChanged(nameof(HabitatDataSource));
            //PetuchText = text as string;
        }
    }
}
