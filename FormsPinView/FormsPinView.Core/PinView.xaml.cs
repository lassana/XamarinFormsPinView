using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace FormsPinView.Core
{
    /// <summary>
    /// The PIN view.
    /// </summary>
    public partial class PinView : Grid
    {
        #region Fields

        private const int DefaultPinLength = 4;
        private const string DefaultEmptyCircleImage = "img_circle.png";
        private const string DefaultFilledCircleImage = "img_circle_filled.png";

        #endregion

        #region Events

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

        #endregion

        #region Bindable properties

        public static readonly BindableProperty TargetPinLengthProperty =
            BindableProperty.Create(propertyName: nameof(TargetPinLength),
                                    returnType: typeof(int),
                                    declaringType: typeof(PinView),
                                    defaultValue: DefaultPinLength);

        /// <summary>
        /// Gets or sets the length of the PIN.
        /// </summary>
        public int TargetPinLength
        {
            get { return (int)GetValue(TargetPinLengthProperty); }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("TargetPinLength must be a positive value");
                }
                SetValue(TargetPinLengthProperty, value);
                DisplayedTextUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public static readonly BindableProperty EmptyCircleImageProperty =
            BindableProperty.Create(propertyName: nameof(EmptyCircleImage),
                                    returnType: typeof(ImageSource),
                                    declaringType: typeof(PinView),
                                    defaultValue: new FileImageSource { File = DefaultEmptyCircleImage });

        /// <summary>
        /// Gets or sets the ImageSource of the <i>empty</i> item icon.
        /// </summary>
        public ImageSource EmptyCircleImage
        {
            get { return (ImageSource)GetValue(EmptyCircleImageProperty); }
            set
            {
                SetValue(EmptyCircleImageProperty, value);
                DisplayedTextUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public static readonly BindableProperty FilledCircleImageProperty =
            BindableProperty.Create(propertyName: nameof(FilledCircleImage),
                                    returnType: typeof(ImageSource),
                                    declaringType: typeof(PinView),
                                    defaultValue: new FileImageSource { File = DefaultFilledCircleImage });

        /// <summary>
        /// Gets or sets the ImageSource of the <i>filled</i> item icon.
        /// </summary>
        public ImageSource FilledCircleImage
        {
            get { return (ImageSource)GetValue(FilledCircleImageProperty); }
            set
            {
                SetValue(FilledCircleImageProperty, value);
                DisplayedTextUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public static readonly BindableProperty ValidatorProperty =
            BindableProperty.Create(propertyName: nameof(Validator),
                                    returnType: typeof(Func<IList<char>, bool>),
                                    declaringType: typeof(PinView),
                                    defaultValue: null);

        /// <summary>
        /// Gets or sets the validator function.
        /// </summary>
        public Func<IList<char>, bool> Validator
        {
            get { return (Func<IList<char>, bool>)GetValue(ValidatorProperty); }
            set { SetValue(ValidatorProperty, value); }
        }

        #endregion

        #region Properties

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
        public Command<string> KeyPressedCommand { get; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FormsPinView.Core.PinView"/> class.
        /// </summary>
        public PinView()
        {
            InitializeComponent();

            // we don't have to unsubscribe
            Error += (sender, e) => DoErrorAnimation();
            DisplayedTextUpdated += (sender, e) => UpdateDisplayedText();

            KeyPressedCommand = new Command<string>(arg =>
            {
                if (Validator == null)
                {
                    throw new InvalidOperationException($"{nameof(Validator)} is not set");
                }

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
                        if (Validator.Invoke(EnteredPin))
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

            foreach (var view in Children)
            {
                if (view is PinItemView pinItem)
                {
                    pinItem.Command = KeyPressedCommand;
                }
            }
        }

        /// <summary>
        /// Updates the displayed PIN icons.
        /// </summary>
        protected virtual void UpdateDisplayedText()
        {
            if (TargetPinLength <= 0)
            {
                // not expected to happen
                throw new InvalidOperationException($"{TargetPinLengthProperty.PropertyName} property" +
                                                    $" is not a positive value but {TargetPinLength}");
            }

            if (EnteredPin != null)
            {
                if (circlesStackLayout.Children.Count == 0)
                {
                    for (int i = 0; i < TargetPinLength; ++i)
                    {
                        circlesStackLayout.Children.Add(new Image
                        {
                            Source = EmptyCircleImage,
                            HeightRequest = 28,
                            WidthRequest = 28,
                            MinimumWidthRequest = 28,
                            MinimumHeightRequest = 28
                        });
                    }
                }
                else
                {
                    for (int i = 0; i < EnteredPin.Count; ++i)
                    {
                        (circlesStackLayout.Children[i] as Image).Source = FilledCircleImage;
                    }
                    for (int i = EnteredPin.Count; i < TargetPinLength; ++i)
                    {
                        (circlesStackLayout.Children[i] as Image).Source = EmptyCircleImage;
                    }
                }
            }
        }

        /// <summary>
        /// "Shakes" the PIN view.
        /// </summary>
        protected virtual void DoErrorAnimation()
        {
            this.AbortAnimation("shake");
            this.Animate("shake",
                         (arg) =>
                         {
                             var shift = Math.Sin(2 * 2 * Math.PI * arg);
                             this.TranslationX = 6 * shift;
                         },
                         16 * 4,
                         250,
                         Easing.Linear,
                         (arg1, arg2) =>
                         {
                             this.TranslationX = 0;
                         });
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyNames">Properties names.</param>
        protected void RaisePropertyChanged(params string[] propertyNames)
        {
            if (propertyNames == null)
            {
                return;
            }
            foreach (string propertyName in propertyNames)
            {
                base.OnPropertyChanged(propertyName);
            }
        }
    }
}

