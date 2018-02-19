using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FormsPinView.Droid;
using FormsPinViewSample.Core;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Java.Lang;

namespace FormsPinViewSample.Droid
{
    [Activity(Label = "FormsPinView Sample",
           MainLauncher = true,
           Icon = "@mipmap/icon",
           Theme = "@style/MyTheme",
           ConfigurationChanges = (ConfigChanges.ScreenSize | ConfigChanges.Orientation)
           )]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            FormsAppCompatActivity.TabLayoutResource = Resource.Layout.Tabbar;
            FormsAppCompatActivity.ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);
            Forms.Init(this, bundle);
            PinItemViewRenderer.Init();
            this.LoadApplication(new App());
        }
    }
}