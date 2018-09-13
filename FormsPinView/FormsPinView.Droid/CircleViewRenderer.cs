using System;
using System.ComponentModel;
using Android.Graphics;
using FormsPinView.Core;
using FormsPinView.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AView = Android.Views.View;

[assembly: ExportRenderer(typeof(CircleView), typeof(CircleViewRenderer))]
namespace FormsPinView.Droid
{
    /// <summary>
    /// <see cref="CircleView"/> renderer.
    /// </summary>
    public class CircleViewRenderer : ViewRenderer<CircleView, AView>
    {
        private float _lineWidth;
        private Paint _strokePaint;
        private Paint _fillPaint;
        private bool _isFilledUp;

        public CircleViewRenderer(Android.Content.Context context) : base(context)
        {
            SetWillNotDraw(false);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CircleView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            _strokePaint = new Paint();
            _strokePaint.SetStyle(Paint.Style.Stroke);
            _strokePaint.AntiAlias = true;
            _strokePaint.Color = Element.Color.ToAndroid();
            _strokePaint.StrokeWidth = _lineWidth = Context.ToPixels(1d);

            _fillPaint = new Paint();
            _fillPaint.SetStyle(Paint.Style.Fill);
            _fillPaint.Color = Element.Color.ToAndroid();

            _isFilledUp = false;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_strokePaint != null)
            {
                if (e.PropertyName == CircleView.ColorProperty.PropertyName)
                {
                    _strokePaint.Color
                        = _fillPaint.Color
                        = Element.Color.ToAndroid();
                    Invalidate();
                    return;
                }
                else if (e.PropertyName == CircleView.IsFilledUpProperty.PropertyName)
                {
                    _isFilledUp = Element.IsFilledUp;
                    Invalidate();
                    return;
                }
            }
            base.OnElementPropertyChanged(sender, e);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            int paddingLeft = PaddingLeft;
            int paddingTop = PaddingTop;
            int paddingRight = PaddingRight;
            int paddingBottom = PaddingBottom;

            int contentWidth = Width - paddingLeft - paddingRight;
            int contentHeight = Height - paddingTop - paddingBottom;
            float radius = Math.Min(contentWidth, contentHeight) / 2 - _lineWidth;

            canvas.DrawCircle(
                cx: paddingLeft + contentWidth / 2,
                cy: paddingTop + contentHeight / 2,
                radius: radius,
                paint: _isFilledUp ? _fillPaint : _strokePaint);
        }
    }
}