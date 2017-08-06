namespace FormsPinViewFSample.ViewModels

open System
open System.Windows.Input
open Xamarin.Forms

type MainViewModel() = 
    let githubUri = Uri("https://github.com/lassana/XamarinFormsPinView");

    member this.GithubCommand: ICommand = Command(fun () -> Device.OpenUri(githubUri)) :> ICommand