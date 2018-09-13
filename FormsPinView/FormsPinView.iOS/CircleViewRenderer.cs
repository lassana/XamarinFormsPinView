using System;
using System.ComponentModel;
using CoreGraphics;
using FormsPinView.Core;
using FormsPinView.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CircleView), typeof(CircleViewRenderer))]
namespace FormsPinView.iOS
{
    /// <summary>
    /// <see cref="CircleView"/> renderer.
    /// </summary>
    public class CircleViewRenderer : ViewRenderer<CircleView, UIView>
    {
        private readonly nfloat _lineWidth = 1f;

        public CGColor StrokeColor => Element.Color.ToCGColor();

        public CGColor FillColor => Element.IsFilledUp ? Element.Color.ToCGColor() : UIColor.Clear.CGColor;

        protected override void OnElementChanged(ElementChangedEventArgs<CircleView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Layer.MasksToBounds = true;
                Layer.CornerRadius = NMath.Min((float)e.NewElement.WidthRequest,
                                               (float)e.NewElement.HeightRequest) / 2.0f;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CircleView.IsFilledUpProperty.PropertyName
                || e.PropertyName == CircleView.ColorProperty.PropertyName)
            {
                this.SetNeedsDisplay();
                return;
            }
            base.OnElementPropertyChanged(sender, e);
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            using (var g = UIGraphics.GetCurrentContext())
            {
                g.SetLineWidth(_lineWidth);
                g.SetFillColor(FillColor);
                g.SetStrokeColor(StrokeColor);

                CGPath path = new CGPath();
                path.AddArc(x: rect.X + (rect.Width / 2),
                            y: rect.Y + (rect.Height / 2),
                            radius: NMath.Min(rect.Width, rect.Height) / 2 - _lineWidth,
                            startAngle: 0f,
                            endAngle: 2.0f * NMath.PI,
                            clockwise: true);
                g.AddPath(path);
                g.DrawPath(CGPathDrawingMode.FillStroke);
            }
        }
    }
}