using System.Windows.Input;
using Xamarin.Forms;

namespace FormsPinView.Core
{
    /// <summary>
    /// The PIN item view (a button).
    /// </summary>
    public class PinItemView : ContentView
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
    }
}