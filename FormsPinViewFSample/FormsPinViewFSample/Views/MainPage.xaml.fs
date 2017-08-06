namespace FormsPinViewFSample.Views

open FormsPinViewFSample.ViewModels
open Xamarin.Forms
open Xamarin.Forms.Xaml

type MainPage() = 
    inherit ContentPage()
    let _ = base.LoadFromXaml(typeof<MainPage>)
    do
        base.BindingContext <- MainViewModel()