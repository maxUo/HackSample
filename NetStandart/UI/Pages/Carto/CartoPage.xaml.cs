using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Syncfusion.SfMaps.XForms;
using UnblockHackNET.BL.ViewModels.Carto;
using Xamarin.Forms;

namespace UnblockHackNET.UI.Pages
{


    public partial class CartoPage : BasePage
    {


        public CartoPage()
        {
            InitializeComponent();
        }


        protected override void OnAppearing()
        {

            if (BindingContext is CartoViewModel)
            {
                var tmp = BindingContext as CartoViewModel;
                tmp.OnAppear?.Execute(null);

                SfMaps map = new SfMaps();
                map.BackgroundColor = Color.White;
                //var map = this.Content as SfMaps;

                ShapeFileLayer layer = new ShapeFileLayer();

                layer.Uri = "world1.shp";

                layer.ItemsSource = tmp.HabitatDataSource;
                //layer.SetBinding(ShapeFileLayer.ItemsSourceProperty, "HabitatDataSource");

                layer.ShapeIDTableField = "POSTAL";
                layer.ShapeIDPath = "Postal";


                EqualColorMapping colorMapping = new EqualColorMapping();
                colorMapping.Color = Color.FromHex("#D84444");
                colorMapping.Value = "AnimalLabel";

                ShapeSetting shapeSetting = new ShapeSetting();
                shapeSetting.ShapeValuePath = "Animal";
                shapeSetting.ShapeColorValuePath = "Animal";
                shapeSetting.ColorMappings.Add(colorMapping);
                layer.ShapeSettings = shapeSetting;

                map.Layers.Add(layer);
                this.Content = map;
            }
            base.OnAppearing();
        }
    }


}
