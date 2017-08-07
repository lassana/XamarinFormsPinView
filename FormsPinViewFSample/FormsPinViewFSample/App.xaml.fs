namespace FormsPinViewFSample

open FormsPinViewFSample.Views
open Xamarin.Forms

type App() =
    inherit Application()
    do
        let rootPage = NavigationPage(MainPage())
        let nextPage = PinAuthPage()
        rootPage.Navigation.PushModalAsync nextPage |> ignore
        base.MainPage <- rootPage

    override this.OnStart() =
        System.Diagnostics.Debug.WriteLine "OnStart"

    override this.OnSleep() =
        System.Diagnostics.Debug.WriteLine "OnSleep"

    override this.OnResume() =
        System.Diagnostics.Debug.WriteLine "OnResume"