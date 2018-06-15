# PIN keyboard for Xamarin.Forms.

<pre><code><img src="ios.mov.gif" height="auto" width="220px"> <img src="android.mov.gif" height="auto" width="220px"></code></pre>

## Usage

1. Add the following NuGet package: https://www.nuget.org/packages/FormsPinView/ [![NuGet](https://img.shields.io/nuget/v/FormsPinView.svg?label=NuGet)](https://www.nuget.org/packages/FormsPinView/) 
    
    _OR_
    
    add [FormsPinView.PCL](FormsPinView/FormsPinView.PCL), [FormsPinView.iOS](FormsPinView/FormsPinView.iOS), and [FormsPinView.Droid](FormsPinView/FormsPinView.Droid) to your solution.
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

1. Add PinView to your page:
        
        ...
        xmlns:local="clr-namespace:FormsPinView.PCL;assembly=FormsPinView.PCL"
        ...
            <local:PinView
                Title="🔐 Enter your PIN"
                HorizontalOptions="CenterAndExpand"
                VerticalOptions="CenterAndExpand"
                BindingContext="{Binding PinViewModel}" />
        
1. `PinView` is MVVM-ready, so you may inherit from `PinViewModel`:


        public class YourPageViewModel
        {
            ...
            
            public PinViewModel PinViewModel { get; private set; } = new PinViewModel
            {
                TargetPinLength = 4,
                ValidatorFunc = (arg) => 
                {
                    //TODO Check entered pin
                    return true;
                }
            };
            
            ...
        }

## TODO

[ ] `AbsoluteLayout` instead of `Grid`
[ ] Colorizing
[ ] Randomizing the numbers order
[ ] Nightly NuGets

## Changelog

### 2.0-pre1 (coming soon)

- Removed `Title` property: now you have to implement it manually in your UI
- Refactored the ViewModel API (splitted into bindable properties)
- Namespaces changed from `PCL` to `Core`.
- Allowed to change the PIN length dynamically

### 1.1.1 (coming soon)

- 1.1.1-pre1 released as a stable version

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
