namespace FormsPinViewSample.PCL.Views

open System
open FormsPinViewSample.PCL.ViewModels
open Xamarin.Forms
open Xamarin.Forms.Xaml

type PinAuthPage() as this =
    inherit ContentPage()
    let _ = base.LoadFromXaml(typeof<PinAuthPage>)

    do
        NavigationPage.SetHasNavigationBar(this, false)
        let viewModel = PinAuthViewModel()
        let handler = 
            EventHandler(fun s o -> 
                this.Navigation.PopAsync() |> ignore
            )
        viewModel.PinViewModel.Success.AddHandler handler
        base.BindingContext <- viewModel

    override this.OnBackButtonPressed(): bool = false