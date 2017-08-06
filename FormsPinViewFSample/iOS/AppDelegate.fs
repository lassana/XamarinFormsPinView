namespace FormsPinViewFSample.iOS

open UIKit
open FormsPinView.iOS
open FormsPinViewFSample
open Foundation
open Xamarin.Forms
open Xamarin.Forms.Platform.iOS

[<Register ("AppDelegate")>]
type AppDelegate () =
    inherit FormsApplicationDelegate ()

    override this.FinishedLaunching(app, options) =
        Forms.Init()
        PinItemViewRenderer.Init()
        this.LoadApplication(new App())
        base.FinishedLaunching(app, options)

module Main =
    [<EntryPoint>]
    let main args =
        UIApplication.Main(args, null, "AppDelegate")
        0

