# PIN keyboard for Xamarin.Forms

<pre><code><img src="ios.mov.gif" height="auto" width="220px"> <img src="android.mov.gif" height="auto" width="220px"></code></pre>

## Usage

1. Add the following NuGet package: https://www.nuget.org/packages/FormsPinView/ [![NuGet](https://img.shields.io/nuget/v/FormsPinView.svg?label=NuGet)](https://www.nuget.org/packages/FormsPinView/) 
    
    _OR_
    
    add [FormsPinView.Core](FormsPinView/FormsPinView.Core), [FormsPinView.iOS](FormsPinView/FormsPinView.iOS), and [FormsPinView.Droid](FormsPinView/FormsPinView.Droid) to your solution.
1. Initialize iOS and Android renderers:

        // iOS:
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            ...
            global::Xamarin.Forms.Forms.Init();
            PinItemViewRenderer.Init();
        }
        
        // Android:
        protected override void OnCreate(Bundle bundle)
        {
            ...
            global::Xamarin.Forms.Forms.Init(this, bundle);
            PinItemViewRenderer.Init();
        }

1. Add `PinView` to your page:
        
        ...
        xmlns:local="clr-namespace:FormsPinView.Core;assembly=FormsPinView.Core"
        ...
            <local:PinView
                HorizontalOptions="CenterAndExpand"
                VerticalOptions="CenterAndExpand"
                TargetPinLength="4"
                Validator="{Binding ValidatorFunc}"
                Success="Handle_Success" />
        
1. `PinView` is MVVM-ready, so you can bind the following properties:

- `Validator` (`Func<IList<char>, bool>`) - required; you should check entered PIN there
- `PinLength` (`int`) - the PIN length; default is 4
- `EmptyCircleImage` (`ImageSource`) - _not entered_ symbol representation; default is a black empty circle
- `FilledCircleImage` (`ImageSource`) - _entered_ symbol representation; default is a black filled circle
- `SuccessCommand` (`ICommand`) - invoked when the correct PIN is entered
- `ErrorCommand` (`ICommand`) - invoked when an incorrect PIN is entered
- `ClearAfterSuccess` (`bool`) - indicates whether the entered PIN should be cleaned or not after it was confirmed as correct; default is `true` 

## TODO

- [ ] Use `AbsoluteLayout` instead of `Grid`, no XAML
- [ ] Colorizing
- [ ] Randomizing the numbers order
- [ ] CI builds
- [ ] UI tests

## Changelog

### 2.0.0-pre1 (coming soon)

- Removed `Title` property: now you have to implement it manually in your UI
- Refactored the ViewModel API (splitted into bindable properties)
- Namespaces changed from `PCL` to `Core`.
- Allowed to change the PIN length dynamically as well as PIN symbols

### 1.1.1 (coming soon)

- *1.1.1-pre1* released as a stable version

### 1.1.1-pre1

- Fix `NSInternalInconsistencyException` crash on iOS

### 1.1.0

- .NET Standard 2.0 is now supported
- PCL support is dropped

### 1.0.1

- PCL profile changed to 259
- F# sample added

### 1.0.0

- Intilial release 

## License

BSD 2-Clause.
