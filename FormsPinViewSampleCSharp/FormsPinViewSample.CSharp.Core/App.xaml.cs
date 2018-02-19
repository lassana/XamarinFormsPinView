using FormsPinViewSample.Core.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace FormsPinViewSample.Core
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var rootPage = new NavigationPage(new MainPage());
            var nextPage = new PinAuthPage();
            rootPage.Navigation.PushAsync(nextPage);
            base.MainPage = rootPage;
        }

        protected override void OnStart() =>
            System.Diagnostics.Debug.WriteLine("OnStart");

        protected override void OnSleep() =>
            System.Diagnostics.Debug.WriteLine("OnSleep");

        protected override void OnResume() =>
            System.Diagnostics.Debug.WriteLine("OnResume");
    }
}