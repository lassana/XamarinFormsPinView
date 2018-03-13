using FormsPinView.iOS;
using FormsPinViewSample.Core;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace FormsPinViewSample.CSharp.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            Forms.Init();
            PinItemViewRenderer.Init();
            LoadApplication(new App());
            return base.FinishedLaunching(uiApplication, launchOptions);
        }
    }
}

