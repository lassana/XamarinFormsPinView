using System;
using System.Collections.Generic;
using FormsPinViewSample.ViewModels;
using Xamarin.Forms;

namespace FormsPinViewSample.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainViewModel();
        }
    }
}

