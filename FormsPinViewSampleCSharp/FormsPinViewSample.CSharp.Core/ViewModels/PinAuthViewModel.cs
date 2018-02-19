using FormsPinView.PCL;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections;

namespace FormsPinViewSample.Core.ViewModels
{
    public class PinAuthViewModel : ViewModelBase
    {
        private readonly char[] _correctPin;
        private readonly PinViewModel _pinViewModel;

        public PinViewModel PinViewModel => _pinViewModel;

        public PinAuthViewModel()
        {
            _correctPin = new[] { '1', '2', '3', '4' };
            _pinViewModel = new PinViewModel
            {
                TargetPinLength = 4,
                ValidatorFunc = (arg) => Enumerable.SequenceEqual(arg, _correctPin)
            };
            _pinViewModel.Success += (object sender, EventArgs e) =>
            {
                Debug.WriteLine("Success. Assume page will be closed automatically.");
            };
        }
    }
}