using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace FormsPinView.PCL
{
    public class PinViewModel : INotifyPropertyChanged
    {
        public event EventHandler Success;
        public event EventHandler Error;
        public event EventHandler DisplayedTextUpdated;

        public event PropertyChangedEventHandler PropertyChanged;
        private string _passwordDisplayedText = string.Empty;
        public string PasswordDisplayedText
        {
            get { return _passwordDisplayedText; }
            private set
            {
                _passwordDisplayedText = value;
                RaisePropertyChanged(nameof(PasswordDisplayedText));
            }
        }

        private int _targetPinLength;
        public int TargetPinLength
        {
            get { return _targetPinLength; }
            set
            {
                _targetPinLength = value;
                RaisePropertyChanged(nameof(TargetPinLength));
                DisplayedTextUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        private Func<IList<char>, bool> _validatorFunc;
        public Func<IList<char>, bool> ValidatorFunc
        {
            get { return _validatorFunc; }
            set
            {
                _validatorFunc = value;
                RaisePropertyChanged(nameof(ValidatorFunc));
            }
        }

        private IList<char> _enteredPin = new List<char>();
        public IList<char> EnteredPin
        {
            get { return _enteredPin; }
            set
            {
                _enteredPin = value;
                RaisePropertyChanged(nameof(EnteredPin));
            }
        }

        public Command<string> KeyPressCommand { get; private set; }

        public PinViewModel()
        {
            KeyPressCommand = new Command<string>(arg =>
            {
                if (arg == "Backspace")
                {
                    if (EnteredPin.Count > 0)
                    {
                        EnteredPin.RemoveAt(EnteredPin.Count - 1);
                        DisplayedTextUpdated?.Invoke(this, EventArgs.Empty);
                    }
                }
                else if (EnteredPin.Count < TargetPinLength)
                {
                    EnteredPin.Add(arg[0]);
                    if (EnteredPin.Count == TargetPinLength)
                    {
                        if (ValidatorFunc.Invoke(EnteredPin))
                        {
                            EnteredPin.Clear();
                            Success?.Invoke(this, EventArgs.Empty);
                            DisplayedTextUpdated?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            EnteredPin.Clear();
                            Error?.Invoke(this, EventArgs.Empty);
                            DisplayedTextUpdated?.Invoke(this, EventArgs.Empty);
                        }
                    }
                    else
                    {
                        DisplayedTextUpdated?.Invoke(this, EventArgs.Empty);
                    }
                }
            });
        }

        protected void RaisePropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null && propertyNames != null)
            {
                foreach (string propertyName in propertyNames)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}

