using PinView.Droid;
using PinView.PCL;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;
using AView = Android.Views.View;
using AButton = Android.Widget.Button;
using AColor = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(PinItemView), typeof(PinItemViewRenderer))]
namespace PinView.Droid
{
    public class PinItemViewRenderer : ViewRenderer<PinItemView, AView>
    {
        private RippleButton _button;

        public static void Init()
        {
            //var t = typeof(PinItemViewRenderer);
        }

        public PinItemViewRenderer()
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<PinItemView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {

            }

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    _button = new RippleButton(Context);
                    _button.SetBackgroundColor(AColor.Transparent);
                    _button.Text = Element.Text;
                    _button.Gravity = Android.Views.GravityFlags.Center;
                    _button.OnClick += (sender, args) => 
                    {
                        Element?.Command?.Execute(Element?.CommandParameter);
                    };
                    SetNativeControl(_button);
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        protected override AView CreateNativeControl()
        {
            return _button;
        }
    }
}

