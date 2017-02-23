using System;
using System.Collections.Generic;
using PinViewSample.ViewModels;
using Xamarin.Forms;

namespace PinViewSample.Views
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

