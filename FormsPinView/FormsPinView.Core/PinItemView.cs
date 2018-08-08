using System.Windows.Input;
using Xamarin.Forms;

namespace FormsPinView.Core
{
    /// <summary>
    /// The PIN item view (a button).
    /// </summary>
    public class PinItemView : View
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(Button), null);

        /// <summary>
        /// Gets or sets the item text.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(PinItemView), null);

        /// <summary>
        /// Gets or sets the item command.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(Button), null);

        /// <summary>
        /// Gets or sets the item command parameter.
        /// </summary>
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(propertyName: nameof(BorderColor),
                                    returnType: typeof(Color),
                                    declaringType: typeof(PinItemView),
                                    defaultValue: Color.Gray);
        
        /// <summary>
        /// Gets or sets a view border color.
        /// </summary>
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create(propertyName: nameof(Color),
                                    returnType: typeof(Color),
                                    declaringType: typeof(PinItemView),
                                    defaultValue: Color.Black);

        /// <summary>
        /// Gets or sets a view basic color.
        /// </summary>
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly BindableProperty RippleColorProperty =
            BindableProperty.Create(propertyName: nameof(RippleColor),
                                    returnType: typeof(Color),
                                    declaringType: typeof(PinItemView),
                                    defaultValue: Color.Gray);

        /// <summary>
        /// Gets or sets a view ripple color.
        /// </summary>
        public Color RippleColor
        {
            get { return (Color)GetValue(RippleColorProperty); }
            set { SetValue(RippleColorProperty, value); }
        }
    }
}