using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace PinView.PCL
{
    public class PinViewModel : INotifyPropertyChanged
    {

        public EventHandler<EventArgs> OnSuccess;
        public EventHandler<EventArgs> OnError;
        public EventHandler<EventArgs> OnUpdateDisplayedText;

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

        private IList<char> _targetPin = new List<char>();
        public IList<char> TargetPin
        {
            get { return _targetPin; }
            set
            {
                _targetPin = value;
                RaisePropertyChanged(nameof(TargetPin));
                OnUpdateDisplayedText?.Invoke(this, EventArgs.Empty);
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
                        OnUpdateDisplayedText?.Invoke(this, EventArgs.Empty);
                    }
                }
                else if (EnteredPin.Count < TargetPin.Count)
                {
                    EnteredPin.Add(arg[0]);
                    if (EnteredPin.Count == TargetPin.Count)
                    {
                        if (EnteredPin.SequenceEqual(TargetPin))
                        {
                            EnteredPin.Clear();
                            OnSuccess?.Invoke(this, EventArgs.Empty);
                            OnUpdateDisplayedText?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            EnteredPin.Clear();
                            OnError?.Invoke(this, EventArgs.Empty);
                            OnUpdateDisplayedText?.Invoke(this, EventArgs.Empty);
                        }
                    }
                    else
                    {
                        OnUpdateDisplayedText?.Invoke(this, EventArgs.Empty);
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

