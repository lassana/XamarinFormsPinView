using System;
using System.Collections.Generic;

using Xamarin.Forms;
using FormsPinViewSample.ViewModels;

namespace FormsPinViewSample.Views
{
    public partial class PinAuthPage : ContentPage
    {
        public PinAuthPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            var vm = new PinAuthViewModel();

            vm.PinViewModel.Success += (sender, e) =>
            {
                Navigation.PopAsync();
            };

            BindingContext = vm;
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }
    }
}

