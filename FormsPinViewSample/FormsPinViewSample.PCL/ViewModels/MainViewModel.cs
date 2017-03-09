using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace FormsPinViewSample.ViewModels
{
    public class MainViewModel
    {
        public ICommand GithubCommand { get; } = new Command(() => 
        { 
            Device.OpenUri(new Uri("https://github.com/lassana/XamarinFormsPinView"));
        });
    }
}

