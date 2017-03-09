using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace FormsPinView.iOS
{
    public class ZFRippleButton : UIButton
    {
        private UIView _rippleView = new UIView();
        private UIView _rippleBackgroundView = new UIView();

        public float _ripplePercent = 0.8f;
        public float RipplePercent
        {
            get { return _ripplePercent; }
            set
            {
                _ripplePercent = value;
                SetupRippleView();
            }
        }

        private UIColor _rippleColor;
        public UIColor RippleColor
        {
            get { return _rippleColor; }
            set
            {
                _rippleColor = value;
                _rippleView.BackgroundColor = RippleColor;
            }
        }

        private UIColor _rippleBackgroundColor;
        public UIColor RippleBackgroundColor
        {
            get { return _rippleBackgroundColor; }
            set
            {
                _rippleBackgroundColor = value;
                _rippleView.BackgroundColor = value;
            }
        }

        private float _buttonCornerRadius = 0f;
        public float ButtonCornerRadius
        {
            get { return _buttonCornerRadius; }
            set
            {
                _buttonCornerRadius = value;
                Layer.CornerRadius = value;
            }
        }


        public bool RippleOverBounds { get; set; } = false;
        public float ShadowRippleRadius { get; set; } = 1f;
        public bool ShadowRippleEnable { get; set; } = true;
        public bool TrackTouchLocation { get; set; } = false;
        public double TouchUpAnimationTime { get; set; } = 0.6d;


        private nfloat _tempShadowRadius = 0f;
        private float _tempShadowOpacity = 0f;
        private CGPoint? _touchCenterLocation;

        private CAShapeLayer RippleMask
        {
            get
            {
                if (!RippleOverBounds)
                {
                    var maskLayer = new CAShapeLayer();
                    maskLayer.Path = UIBezierPath.FromRoundedRect(Bounds, Layer.CornerRadius).CGPath;
                    return maskLayer;
                }
                else
                {
                    return null;
                }
            }
        }

        public ZFRippleButton(CGRect frame) : base(frame)
        {
            Setup();
        }

        private void Setup()
        {
            SetupRippleView();

            _rippleBackgroundView.BackgroundColor = RippleBackgroundColor;
            _rippleBackgroundView.Frame = Bounds;
            Layer.AddSublayer(_rippleBackgroundView.Layer);

            _rippleBackgroundView.Layer.AddSublayer(_rippleView.Layer);
            _rippleBackgroundView.Alpha = 0;


            Layer.ShadowRadius = 0;
            Layer.ShadowOffset = new CGSize(width: 0, height: 1);
            Layer.ShadowColor = new UIColor(white: 0.0f, alpha: 0.5f).CGColor;
        }

        private void SetupRippleView()
        {
            var size = Bounds.Width * RipplePercent;
            var x = (Bounds.Width / 2) - (size / 2);
            var y = (Bounds.Height / 2) - (size / 2);
            var corner = size / 2;

            _rippleView.BackgroundColor = RippleColor;
            _rippleView.Frame = new CGRect(x, y, size, size);
            _rippleView.Layer.CornerRadius = corner;
        }

        public override bool BeginTracking(UITouch touch, UIEvent uievent)
        {
            if (TrackTouchLocation)
            {
                _touchCenterLocation = touch.LocationInView(this);
            }
            else {
                _touchCenterLocation = null;
            }

            UIView.Animate(0.1, 0, UIViewAnimationOptions.AllowUserInteraction, () =>
            {
                _rippleBackgroundView.Alpha = 1;
            }, null);
            _rippleView.Transform = CGAffineTransform.MakeScale(0.5f, 0.5f);

            UIView.Animate(0.7, 0, UIViewAnimationOptions.CurveEaseOut | UIViewAnimationOptions.AllowUserInteraction, () =>
            {
                _rippleView.Transform = CGAffineTransform.MakeIdentity();
            }, null);

            if (ShadowRippleEnable)
            {
                _tempShadowRadius = Layer.ShadowRadius;
                _tempShadowOpacity = Layer.ShadowOpacity;

                var shadowAnim = new CABasicAnimation { KeyPath = "shadowRadius" };
                shadowAnim.To = NSValue.FromObject(ShadowRippleRadius);

                var opacityAnim = new CABasicAnimation { KeyPath = "shadowOpacity" };
                opacityAnim.To = NSValue.FromObject(1);

                var groupAnim = new CAAnimationGroup();
                groupAnim.Duration = 0.7;
                groupAnim.FillMode = CAFillMode.Forwards;
                groupAnim.RemovedOnCompletion = false;
                groupAnim.Animations = new[] { shadowAnim, opacityAnim };
                Layer.AddAnimation(groupAnim, "shadow");
            }
            return base.BeginTracking(touch, uievent);
        }

        public override void CancelTracking(UIEvent uievent)
        {
            base.CancelTracking(uievent);
            AnimateToNormal();
        }

        public override void EndTracking(UITouch uitouch, UIEvent uievent)
        {
            base.EndTracking(uitouch, uievent);
            AnimateToNormal();
        }

        private void AnimateToNormal()
        {
            UIView.Animate(0.1, 0, UIViewAnimationOptions.AllowUserInteraction, () =>
            {
                _rippleBackgroundView.Alpha = 1;
            }, () =>
            {
                UIView.Animate(TouchUpAnimationTime, 0, UIViewAnimationOptions.AllowUserInteraction, () =>
                {
                    _rippleBackgroundView.Alpha = 0;
                }, null);
            });

            UIView.Animate(0.7, 0, UIViewAnimationOptions.CurveEaseOut | UIViewAnimationOptions.BeginFromCurrentState | UIViewAnimationOptions.AllowUserInteraction, () =>
            {
                _rippleView.Transform = CGAffineTransform.MakeIdentity();

                var shadowAnim = new CABasicAnimation { KeyPath = "shadowRadius" };
                shadowAnim.To = NSObject.FromObject(_tempShadowRadius);

                var opacityAnim = new CABasicAnimation { KeyPath = "shadowOpacity" };
                opacityAnim.To = NSObject.FromObject(_tempShadowOpacity);

                var groupAnim = new CAAnimationGroup();
                groupAnim.Duration = 0.7;
                groupAnim.FillMode = CAFillMode.Forwards;
                groupAnim.RemovedOnCompletion = false;
                groupAnim.Animations = new[] { shadowAnim, opacityAnim };

                Layer.AddAnimation(groupAnim, "shadowBack");
            }, null);
        }
    }
}

