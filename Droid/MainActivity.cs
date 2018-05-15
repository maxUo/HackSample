using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using UnblockHackNET;
using Acr.UserDialogs;
using Xamarin.Forms;
using CarouselView.FormsPlugin.Android;
using Plugin.Permissions;
using Plugin.CurrentActivity;
using Plugin.Permissions.Abstractions;
using Syncfusion.SfMaps;
using Syncfusion.SfMaps.XForms.Droid;

namespace UnblockHackMobile.Droid
{
    [Activity(Label = "LionSample", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            UserDialogs.Init(() => (Activity) Forms.Context);


            global::Xamarin.Forms.Forms.Init(this, bundle);
			CarouselViewRenderer.Init();
		
         
            LoadApplication(new App());
        }

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
