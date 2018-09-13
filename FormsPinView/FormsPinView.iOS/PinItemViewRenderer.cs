using System.ComponentModel;
using CoreGraphics;
using FormsPinView.Core;
using FormsPinView.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PinItemView), typeof(PinItemViewRenderer))]
namespace FormsPinView.iOS
{
    /// <summary>
    /// <see cref="PinItemView"/> renderer.
    /// </summary>
    public class PinItemViewRenderer : ViewRenderer<PinItemView, UIView>
    {
        private ZFRippleButton _button;

        public static new void Init()
        {
            _ = typeof(PinItemViewRenderer);
            _ = typeof(CircleViewRenderer);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<PinItemView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {

            }
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    _button = new ZFRippleButton(new CGRect(0, 0, 44, 44));
                    _button.BackgroundColor = UIColor.Clear;
                    _button.SetTitle(Element.Text, UIControlState.Normal);
                    _button.SetTitleColor(Element.Color.ToUIColor(), UIControlState.Normal);
                    _button.ClipsToBounds = true;
                    _button.Layer.CornerRadius = _button.Bounds.Size.Height / 2;
                    _button.Layer.BorderColor = Element.BorderColor.ToCGColor();
                    _button.Layer.BorderWidth = 1f;

                    _button.RippleColor
                        = _button.RippleBackgroundColor
                        = Element.RippleColor.ToUIColor();
                    _button.RippleOverBounds = true;
                    _button.ShadowRippleEnable = true;
                    _button.ShadowRippleRadius = 32;
                    _button.RipplePercent = 1;

                    _button.TouchUpInside += (sender, args) =>
                    {
                        Element?.Command?.Execute(Element?.CommandParameter);
                    };

                    UIView uiView = new UIView();
                    uiView.AddSubview(_button);
                    uiView.SizeToFit();

                    SetNativeControl(uiView);
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_button != null)
            {
                if (e.PropertyName == PinItemView.ColorProperty.PropertyName)
                {
                    _button.SetTitleColor(Element.Color.ToUIColor(), UIControlState.Normal);
                    return;
                }
                else if (e.PropertyName == PinItemView.BorderColorProperty.PropertyName)
                {
                    _button.Layer.BorderColor = Element.BorderColor.ToCGColor();
                    return;
                }
                else if (e.PropertyName == PinItemView.RippleColorProperty.PropertyName)
                {
                    _button.RippleColor
                        = _button.RippleBackgroundColor
                        = Element.RippleColor.ToUIColor();
                    return;
                }
            }
            base.OnElementPropertyChanged(sender, e);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            _button.Center = Control.Center;
        }
    }
}