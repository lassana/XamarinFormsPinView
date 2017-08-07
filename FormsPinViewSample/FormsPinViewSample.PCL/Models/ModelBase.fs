namespace FormsPinViewSample.PCL.Models

open System
open System.ComponentModel
open Microsoft.FSharp.Quotations.Patterns

type ModelBase() = 
    let propertyChanged = Event<_, _>()

    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member this.PropertyChanged = propertyChanged.Publish 

    member this.NotifyPropertyChanged propertyName = 
        propertyChanged.Trigger(this, PropertyChangedEventArgs propertyName)

    member this.NotifyPropertyChanged propertySelector = 
        match propertySelector with 
        | PropertyGet(Some (Value (instance, _)), property, _) when Object.ReferenceEquals(instance, this) -> 
            this.NotifyPropertyChanged property.Name
        | _ -> invalidOp "Expecting property getter expression only (for example `this.SomeProperty`)."