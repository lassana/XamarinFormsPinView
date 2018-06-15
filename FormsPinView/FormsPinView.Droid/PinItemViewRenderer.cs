using System;
using Android.Widget;
using FormsPinView.Core;
using FormsPinView.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;
using AView = Android.Views.View;

[assembly: ExportRenderer(typeof(PinItemView), typeof(PinItemViewRenderer))]
namespace FormsPinView.Droid
{
    public class PinItemViewRenderer : ViewRenderer<PinItemView, AView>
    {
        private RippleButton _button;

        public static void Init()
        {
            _ = typeof(PinItemViewRenderer);
        }

        public PinItemViewRenderer(Android.Content.Context context) : base(context)
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
                    var sideSize = (int)ConvertDpToPixel(44);

                    _button = new RippleButton(Context);
                    _button.SetWidth(sideSize);
                    _button.SetHeight(sideSize);
                    //_button.SetBackgroundColor(AColor.Red);
                    _button.SetBackgroundResource(Resource.Drawable.bkg_roundedview);
                    _button.Text = Element.Text;
                    _button.Gravity = Android.Views.GravityFlags.Center;
                    _button.OnClick += (sender, args) =>
                    {
                        Element?.Command?.Execute(Element?.CommandParameter);
                    };

                    FrameLayout frame = new FrameLayout(Context);
                    FrameLayout.LayoutParams @params = new FrameLayout.LayoutParams(sideSize, sideSize);
                    @params.Gravity = Android.Views.GravityFlags.Center;
                    frame.AddView(_button, @params);

                    SetNativeControl(frame);
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        private float ConvertDpToPixel(float dp)
        {
            float density = Context.Resources.DisplayMetrics.Density;
            return (int)Math.Round((float)dp * density);
        }
    }
}

