﻿using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace FormsPinView.Core
{
    /// <summary>
    /// The PIN view.
    /// </summary>
    public partial class PinView : Grid
    {
        #region Private fields and properties

        private const int DefaultPinLength = 4;

        private const string DefaultEmptyCircleImage = "img_circle.png";
        private const string DefaultFilledCircleImage = "img_circle_filled.png";

        private StackLayout circlesStackLayout;

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

        public static readonly BindableProperty PinLengthProperty =
            BindableProperty.Create(propertyName: nameof(PinLength),
                                    returnType: typeof(int),
                                    declaringType: typeof(PinView),
                                    defaultValue: DefaultPinLength,
                                    propertyChanged: HandlePinLengthChanged);

        /// <summary>
        /// Gets or sets the length of the PIN.
        /// </summary>
        public int PinLength
        {
            get { return (int)GetValue(PinLengthProperty); }
            set 
            { 
                if ((int)value <= 0)
                {
                    throw new ArgumentException("TargetPinLength must be a positive value");
                }
                SetValue(PinLengthProperty, value);
            }
        }

        private static void HandlePinLengthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if ((int)newValue <= 0)
            {
                throw new ArgumentException("TargetPinLength must be a positive value");
            }
            ((PinView)bindable).EnteredPin.Clear();
            ((PinView)bindable).UpdateDisplayedText(resetUI: true);
        }

        public static readonly BindableProperty EmptyCircleImageProperty =
            BindableProperty.Create(propertyName: nameof(EmptyCircleImage),
                                    returnType: typeof(ImageSource),
                                    declaringType: typeof(PinView),
                                    defaultValue: new FileImageSource { File = DefaultEmptyCircleImage },
                                    propertyChanged: HandleCircleImageChanged);

        /// <summary>
        /// Gets or sets the ImageSource of the <i>empty</i> item icon.
        /// </summary>
        public ImageSource EmptyCircleImage
        {
            get { return (ImageSource)GetValue(EmptyCircleImageProperty); }
            set
            {
                SetValue(EmptyCircleImageProperty, value);

            }
        }

        public static readonly BindableProperty FilledCircleImageProperty =
            BindableProperty.Create(propertyName: nameof(FilledCircleImage),
                                    returnType: typeof(ImageSource),
                                    declaringType: typeof(PinView),
                                    defaultValue: new FileImageSource { File = DefaultFilledCircleImage },
                                    propertyChanged: HandleCircleImageChanged);

        /// <summary>
        /// Gets or sets the ImageSource of the <i>filled</i> item icon.
        /// </summary>
        public ImageSource FilledCircleImage
        {
            get { return (ImageSource)GetValue(FilledCircleImageProperty); }
            set
            {
                SetValue(FilledCircleImageProperty, value);
                UpdateDisplayedText(resetUI: true);
            }
        }

        private static void HandleCircleImageChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((PinView)bindable).UpdateDisplayedText(resetUI: true);
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

        public static readonly BindableProperty SuccessCommandProperty =
            BindableProperty.Create(propertyName: nameof(SuccessCommand),
                                    returnType: typeof(ICommand),
                                    declaringType: typeof(PinView),
                                    defaultValue: null);

        /// <summary>
        /// Gets or sets a command which is invoked when the correct PIN is entered.
        /// </summary>
        public ICommand SuccessCommand
        {
            get { return (ICommand)GetValue(SuccessCommandProperty); }
            set { SetValue(SuccessCommandProperty, value); }
        }

        public static readonly BindableProperty ErrorCommandProperty =
            BindableProperty.Create(propertyName: nameof(ErrorCommand),
                                    returnType: typeof(ICommand),
                                    declaringType: typeof(PinView),
                                    defaultValue: null);

        /// <summary>
        /// Gets or sets a command which is invoked when an incorrect PIN is entered.
        /// </summary>
        public ICommand ErrorCommand
        {
            get { return (ICommand)GetValue(ErrorCommandProperty); }
            set { SetValue(ErrorCommandProperty, value); }
        }

        public static readonly BindableProperty ClearAfterSuccessProperty =
            BindableProperty.Create(propertyName: nameof(ClearAfterSuccess),
                                    returnType: typeof(bool),
                                    declaringType: typeof(PinView),
                                    defaultValue: true);

        /// <summary>
        /// Gets or sets a value indicating whether the entered PIN should be cleaned or not
        /// after it was confirmed as correct. Default is <code>true</code>.
        /// </summary>
        public bool ClearAfterSuccess
        {
            get { return (bool)GetValue(ClearAfterSuccessProperty); }
            set { SetValue(ClearAfterSuccessProperty, value); }
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
        /// The "key pressed" command.
        /// </summary>
        /// <value>The "key pressed" command.</value>
        private Command<string> _keyPressedCommand;
        /// <summary>
        /// Gets the "key pressed" command.
        /// </summary>
        public Command<string> KeyPressedCommand
            => _keyPressedCommand = _keyPressedCommand
            ?? new Command<string>(arg =>
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
                        UpdateDisplayedText(resetUI: false);
                    }
                }
                else if (EnteredPin.Count < PinLength)
                {
                    EnteredPin.Add(arg[0]);
                    if (EnteredPin.Count == PinLength)
                    {
                        if (Validator.Invoke(EnteredPin))
                        {
                            if (ClearAfterSuccess)
                            {
                                EnteredPin.Clear();
                            }
                            // fill the last PIN symbol image
                            UpdateDisplayedText(resetUI: false);
                            // Raise Success event
                            RaiseSuccess();
                        }
                        else
                        {
                            // clear all PIN symbols
                            EnteredPin.Clear();
                            UpdateDisplayedText(resetUI: false);
                            // Raise Error event
                            RaiseError();
                        }
                    }
                    else
                    {
                        UpdateDisplayedText(resetUI: false);
                    }
                }
            });

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FormsPinView.Core.PinView"/> class.
        /// </summary>
        public PinView()
        {
            Resources = new ResourceDictionary
            {
                { "cellHeight", 44d },
                { "cellWidth", 84d }
            };

            RowSpacing = 18;

            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = (double)Resources["cellHeight"] }, // Dots
                new RowDefinition { Height = (double)Resources["cellHeight"] }, // 1 2 3
                new RowDefinition { Height = (double)Resources["cellHeight"] }, // 4 5 6
                new RowDefinition { Height = (double)Resources["cellHeight"] }, // 7 8 9
                new RowDefinition { Height = (double)Resources["cellHeight"] }, //   0 ←
            };
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = (double)Resources["cellWidth"] },
                new ColumnDefinition { Width = (double)Resources["cellWidth"] },
                new ColumnDefinition { Width = (double)Resources["cellWidth"] }
            };

            circlesStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            Children.Add(circlesStackLayout, 0, 3, 0, 1);

            var items = new []
            {
                new []
                {
                    new PinItemView { Text = "1", CommandParameter = "1" },
                    new PinItemView { Text = "2", CommandParameter = "2" },
                    new PinItemView { Text = "3", CommandParameter = "3" }
                },
                new []
                {
                    new PinItemView { Text = "4", CommandParameter = "4" },
                    new PinItemView { Text = "5", CommandParameter = "5" },
                    new PinItemView { Text = "6", CommandParameter = "6" }
                },
                new []
                {
                    new PinItemView { Text = "7", CommandParameter = "7" },
                    new PinItemView { Text = "8", CommandParameter = "8" },
                    new PinItemView { Text = "9", CommandParameter = "9" },
                },
                new []
                {
                    null,
                    new PinItemView { Text = "0", CommandParameter = "0" },
                    new PinItemView { Text = Device.RuntimePlatform == Device.iOS ? "⌫" : "✕", CommandParameter = "Backspace" }
                }
            };
            for (int i = 0; i < items.Length; i++)
            {
                for (int j = 0; j < items[i].Length; j++)
                {
                    if (items[i][j] == null)
                    {
                        continue;
                    }
                    items[i][j].Command = KeyPressedCommand;
                    Children.Add(items[i][j], j, i + 1);
                }
            }

            UpdateDisplayedText(resetUI: true);
        }

        /// <summary>
        /// Updates the displayed PIN icons.
        /// </summary>
        protected void UpdateDisplayedText(bool resetUI)
        {
            if (PinLength <= 0)
            {
                // not expected to happen
                throw new InvalidOperationException($"{PinLengthProperty.PropertyName} property" +
                                                    $" is not a positive value but {PinLength}");
            }

            if (resetUI || circlesStackLayout.Children.Count == 0)
            {
                circlesStackLayout.Children.Clear();
                for (int i = 0; i < PinLength; ++i)
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

            if (EnteredPin != null)
            {
                for (int i = 0; i < EnteredPin.Count; ++i)
                {
                    (circlesStackLayout.Children[i] as Image).Source = FilledCircleImage;
                }
                for (int i = EnteredPin.Count; i < PinLength; ++i)
                {
                    (circlesStackLayout.Children[i] as Image).Source = EmptyCircleImage;
                }
            }

            DisplayedTextUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// "Shakes" the PIN view and raises the <code>Success</code> event.
        /// </summary>
        protected void RaiseError()
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

            var error = Error;
            var errorCommand = ErrorCommand;

            if (error != null)
                error.Invoke(this, EventArgs.Empty);
            
            if (errorCommand != null && errorCommand.CanExecute(null))
            {
                errorCommand.Execute(null);
            }
        }

        /// <summary>
        /// Raises the <code>Success</code> event.
        /// </summary>
        protected void RaiseSuccess()
        {
            var success = Success;
            var successCommand = SuccessCommand;

            if (success != null)
                success.Invoke(this, EventArgs.Empty);

            if (successCommand != null && successCommand.CanExecute(null))
            {
                successCommand.Execute(null);
            }
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