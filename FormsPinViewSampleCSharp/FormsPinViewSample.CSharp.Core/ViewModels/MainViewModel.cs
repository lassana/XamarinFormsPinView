using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace FormsPinViewSample.Core.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Uri _githubUri = new Uri("https://github.com/lassana/XamarinFormsPinView");

        public ICommand GithubCommand => new Command(() => Device.OpenUri(_githubUri));
    }
}