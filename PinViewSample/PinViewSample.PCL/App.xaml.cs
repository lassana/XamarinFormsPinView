using Xamarin.Forms;
using PinViewSample.Views;
using System.Threading.Tasks;

namespace PinViewSample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var rootPage = new NavigationPage(new MainPage());
            var pinPage = new PinAuthPage();
            rootPage.Navigation.PushAsync(pinPage);
            MainPage = rootPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

