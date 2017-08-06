namespace FormsPinViewFSample.Droid

open Android.App
open Android.Content
open Android.Content.PM
open Android.Runtime
open Android.Views
open Android.Widget
open Android.OS
//open FormsPinView.Droid
open FormsPinViewFSample
open System
open Xamarin.Forms
open Xamarin.Forms.Platform.Android

[<Activity (Label = "FormsPinViewFSample.Droid",
            Icon = "@drawable/icon", 
            Theme = "@style/MyTheme",
            MainLauncher = true, 
            ConfigurationChanges = (ConfigChanges.ScreenSize ||| ConfigChanges.Orientation))>]
type MainActivity() =
    inherit FormsAppCompatActivity()
    override this.OnCreate (bundle: Bundle) =
        //TabLayoutResource = Resource.Layout.Tabbar;
        //ToolbarResource = Resource.Layout.Toolbar
        base.OnCreate (bundle)

        Forms.Init (this, bundle)
        //PinItemViewRenderer.Init ();

        this.LoadApplication (new App ())

