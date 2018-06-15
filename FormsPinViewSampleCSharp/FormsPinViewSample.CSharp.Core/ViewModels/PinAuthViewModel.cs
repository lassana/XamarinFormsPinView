using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace FormsPinViewSample.Core.ViewModels
{
    public class PinAuthViewModel : ViewModelBase
    {
        public Func<IList<char>, bool> ValidatorFunc { get; }

        private int _pinLength;
        public int PinLength
        {
            get => _pinLength;
            private set
            {
                _pinLength = value;
                RaisePropertyChanged(nameof(PinLength));
            }
        }

        public ICommand SwitchPinLengthCommand { get; }

        public ICommand ErrorCommand { get; }

        public ICommand SuccessCommand { get; }

        public PinAuthViewModel()
        {
            ValidatorFunc = (arg) =>
            {
                for (int i = 0; i < arg.Count; ++i)
                {
                    if (arg[i] != ('1' + i))
                    {
                        return false;
                    }
                }
                return true;
            };

            PinLength = 4;

            SwitchPinLengthCommand = new Command(() =>
            {
                PinLength = PinLength == 4 ? 6 : 4;
            });

            ErrorCommand = new Command(() =>
            {
                Debug.WriteLine("Entered PIN is wrong");
            });

            SuccessCommand = new Command(() => 
            {
                Debug.WriteLine("Entered PIN is correct");
            });
        }
    }
}