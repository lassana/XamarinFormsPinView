using FormsPinViewSample.Core.ViewModels;
using Xamarin.Forms;

namespace FormsPinViewSample.Core.Views
{
    public partial class PinAuthPage : ContentPage
    {
        public PinAuthPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            var viewModel = new PinAuthViewModel();
            base.BindingContext = viewModel;
        }

        protected override bool OnBackButtonPressed() => false;

        private void Handle_Success(object sender, System.EventArgs e)
        {
            this.Navigation.PopAsync();
        }
    }
}