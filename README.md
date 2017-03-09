# PIN keyboard for Xamarin.Forms.

<pre><code><img src="ios.mov.gif" height="auto" width="220px"> <img src="android.mov.gif" height="auto" width="220px"></code></pre>

## Usage

1. Add [FormsPinView.PCL](FormsPinView/FormsPinView.PCL), [FormsPinView.iOS](FormsPinView/FormsPinView.iOS), and [FormsPinView.Droid](FormsPinView/FormsPinView.Droid) to your solution.
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
        
## License

BSD 2-Clause.
