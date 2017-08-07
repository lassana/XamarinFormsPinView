namespace FormsPinViewSample.PCL.ViewModels

open FormsPinView.PCL
open System
open System.Diagnostics
open System.Linq

type PinAuthViewModel() = 
    inherit ViewModelBase()

    let correctPin = seq [ '1'; '2'; '3'; '4' ]

    let pinViewModel = PinViewModel(TargetPinLength = 4,
                                    ValidatorFunc = fun arg -> 
                                        Enumerable.SequenceEqual(arg, correctPin))

    do 
        let hdlr =
            EventHandler(
                fun sender args -> 
                    Debug.WriteLine "Success. Assume page will be closed automatically."
            )
        pinViewModel.Success.AddHandler hdlr

    member this.PinViewModel = pinViewModel