using FormsPinViewSample.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FormsPinViewSample.Core.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            base.BindingContext = new MainViewModel();
        }
    }
}