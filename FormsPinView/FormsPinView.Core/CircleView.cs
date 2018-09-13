using Xamarin.Forms;

namespace FormsPinView.Core
{
    /// <summary>
    /// Simple circle view.
    /// </summary>
    public class CircleView : View
    {
        public CircleView()
        {
        }

        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create(propertyName: nameof(Color),
                                    returnType: typeof(Color),
                                    declaringType: typeof(CircleView),
                                    defaultValue: Color.Black);

        /// <summary>
        /// Gets or sets the circle color.
        /// </summary>
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly BindableProperty IsFilledUpProperty =
            BindableProperty.Create(propertyName: nameof(IsFilledUp),
                                    returnType: typeof(bool),
                                    declaringType: typeof(CircleView),
                                    defaultValue: false);

        /// <summary>
        /// Gets or sets a value indicating if the circe is filled up.
        /// </summary>
        public bool IsFilledUp
        {
            get { return (bool)GetValue(IsFilledUpProperty); }
            set { SetValue(IsFilledUpProperty, value); }
        }
    }
}