using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace FormsPinView.PCL
{
    /// <summary>
    /// The PivView ViewModel.
    /// </summary>
    public class PinViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when user enters a corrct PIN.
        /// </summary>
        public event EventHandler<EventArgs> Success;

        /// <summary>
        /// Occurs when user enters an incorrect pin.
        /// </summary>
        public event EventHandler<EventArgs> Error;

        /// <summary>
        /// Occurs when user presses a button and displayed text is updated.
        /// </summary>
        public event EventHandler<EventArgs> DisplayedTextUpdated;

        /// <summary>
        /// Occurs when the ViewModel property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private int _targetPinLength = 4; // default is 4
        /// <summary>
        /// Gets or sets the length of the PIN.
        /// </summary>
        public int TargetPinLength
        {
            get { return _targetPinLength; }
            set
            {
                _targetPinLength = value > 0
                    ? value
                    : throw new ArgumentException("TargetPinLength must be a positive value");
                RaisePropertyChanged(nameof(TargetPinLength));
                DisplayedTextUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        private Func<IList<char>, bool> _validatorFunc;
        /// <summary>
        /// Gets or sets the validator function.
        /// </summary>
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
        /// <summary>
        /// Gets or sets the entered PIN.
        /// </summary>
        /// <value>The entered pin.</value>
        public IList<char> EnteredPin
        {
            get { return _enteredPin; }
            set
            {
                _enteredPin = value;
                RaisePropertyChanged(nameof(EnteredPin));
            }
        }

        /// <summary>
        /// Gets the "key pressed" command.
        /// </summary>
        public Command<string> KeyPressCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FormsPinView.PCL.PinViewModel"/> class.
        /// </summary>
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

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyNames">Properties names.</param>
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

