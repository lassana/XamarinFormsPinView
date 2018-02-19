using System;
using FormsPinViewSample.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FormsPinViewSample.Core.Views
{
    public partial class PinAuthPage : ContentPage
    {
        public PinAuthPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            var viewModel = new PinAuthViewModel();
            viewModel.PinViewModel.Success += (object sender, EventArgs e) => 
            {
                this.Navigation.PopAsync();
            };
            base.BindingContext = viewModel;
        }

        protected override bool OnBackButtonPressed() => false;
    }
}