namespace FormsPinViewSample.Droid

open System

open Android.App
open Android.Content
open Android.Content.PM
open Android.Runtime
open Android.Views
open Android.Widget
open Android.OS
open FormsPinView.Droid
open FormsPinViewSampleFSharp.Core
open Xamarin.Forms
open Xamarin.Forms.Platform.Android

type Resources = FormsPinViewSampleFSharp.Droid.Resource

[<Activity (Label = "FormsPinView Sample",
            MainLauncher = true,
            Icon = "@mipmap/icon",
            Theme = "@style/MyTheme",
            ConfigurationChanges = (ConfigChanges.ScreenSize ||| ConfigChanges.Orientation)
            )>]
type MainActivity () =
    inherit FormsAppCompatActivity ()

    override this.OnCreate (bundle) =
        FormsAppCompatActivity.TabLayoutResource <- Resources.Layout.Tabbar;
        FormsAppCompatActivity.ToolbarResource <- Resources.Layout.Toolbar;
        base.OnCreate (bundle)
        Forms.Init(this, bundle)
        PinItemViewRenderer.Init()
        this.LoadApplication(new App())
