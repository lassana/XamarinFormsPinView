using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Util;
using Android.Graphics;
using Android.Content;

namespace PinView.Droid
{
    public class RippleButton : Button
    {
        private float _duration = 250;
        private float _speed = 1;
        private float _radius = 0;
        private Paint _paint = new Paint();
        private float _endRadius = 0;
        private float _rippleX = 0;
        private float _rippleY = 0;
        private int _width = 0;
        private int _height = 0;
        private Handler _handler;
        private MotionEventActions? _touchAction;

        public EventHandler<EventArgs> OnClick;

        public RippleButton(Context context) : base(context)
        {
            Init();
        }

        private void Init()
        {
            if (IsInEditMode)
            {
                return;
            }

            _handler = new Handler();
            _paint.SetStyle(Android.Graphics.Paint.Style.Fill);
            _paint.Color = Color.Gray;
            _paint.AntiAlias = true;
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            _width = w;
            _height = h;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            if (_radius > 0 && _radius < _endRadius)
            {
                canvas.DrawCircle(_rippleX, _rippleY, _radius, _paint);
                if (_touchAction == MotionEventActions.Up)
                {
                    Invalidate();
                }
            }
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            _rippleX = e.GetX();
            _rippleY = e.GetY();

            if (e.Action == MotionEventActions.Up)
            {
                Parent.RequestDisallowInterceptTouchEvent(false);
                _touchAction = MotionEventActions.Up;
                _radius = 1;
                _endRadius = Math.Max(Math.Max(Math.Max(_width - _rippleX, _rippleX), _rippleY), _height - _rippleY);
                _speed = _endRadius / _duration * 10;
                Action animAction = null;
                animAction = () =>
                {
                    if (_radius < _endRadius)
                    {
                        _radius += _speed;
                        _paint.Alpha = 90 - (int)(_radius / _endRadius * 90);
                        _handler.PostDelayed(animAction, 1);
                    }
                    else
                    {
                        OnClick?.Invoke(this, EventArgs.Empty);
                    }
                };
                _handler.PostDelayed(animAction, 10);
                Invalidate();
                return false;
            }
            else if (e.Action == MotionEventActions.Cancel)
            {
                Parent.RequestDisallowInterceptTouchEvent(false);
                _touchAction = MotionEventActions.Cancel;
                _radius = 0;
                Invalidate();
                return false;
            }
            else if (e.Action == MotionEventActions.Down)
            {
                Parent.RequestDisallowInterceptTouchEvent(true);
                _touchAction = MotionEventActions.Up;
                _endRadius = Math.Max(Math.Max(Math.Max(_width - _rippleX, _rippleX), _rippleY), _height - _rippleY);
                _paint.Alpha = 90;
                _radius = _endRadius / 4;
                Invalidate();
                return true;
            }
            else if (e.Action == MotionEventActions.Move)
            {
                if (_rippleX < 0 || _rippleX > _width || _rippleY < 0 || _rippleY > _height)
                {
                    Parent.RequestDisallowInterceptTouchEvent(false);
                    _touchAction = MotionEventActions.Cancel;
                    _radius = 0;
                    Invalidate();
                    return false;
                }
                else
                {
                    _touchAction = MotionEventActions.Move;
                    Invalidate();
                    return true;
                }
            }

            return false;
        }
    }
}

