using System;
using System.Collections.Generic;

using Xamarin.Forms;
using PinViewSample.ViewModels;

namespace PinViewSample.Views
{
    public partial class PinAuthPage : ContentPage
    {
        public PinAuthPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            var vm = new PinAuthViewModel();

            vm.PinViewModel.OnSuccess += (sender, e) =>
            {
                Navigation.PopModalAsync();
            };

            BindingContext = vm;
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }
    }
}

