using System;
using Xamarin.Forms;

namespace PinView.PCL
{
    public partial class PinView : ContentView
    {
        public string Title
        {
            get { return titleLabel.Text; }
            set
            {
                titleLabel.Text = value;
            }
        }

        public PinView()
        {
            InitializeComponent();

            BindingContextChanged += (sender, e) =>
            {
                if (BindingContext is PinViewModel)
                {
                    var vm = BindingContext as PinViewModel;
                    vm.OnError += Handle_OnError;
                    vm.OnUpdateDisplayedText += Handle_OnUpdateDisplayedText;
                    Handle_OnUpdateDisplayedText(vm, EventArgs.Empty);
                }
            };
        }

        private void Handle_OnUpdateDisplayedText(object sender, EventArgs e)
        {
            var vm = sender as PinViewModel;
            if (vm.EnteredPin != null && vm.TargetPin != null)
            {
                if (circlesStackLayout.Children.Count == 0)
                {
                    for (int i = 0; i < vm.TargetPin.Count; ++i)
                    {
                        circlesStackLayout.Children.Add(new Image
                        {
                            Source = "img_circle.png",
                            HeightRequest = 28,
                            WidthRequest = 28
                        });
                    }
                }
                else
                {
                    for (int i = 0; i < vm.EnteredPin.Count; ++i)
                    {
                        (circlesStackLayout.Children[i] as Image).Source = "img_circle_filled.png";
                    }
                    for (int i = vm.EnteredPin.Count; i < vm.TargetPin.Count; ++i)
                    {
                        (circlesStackLayout.Children[i] as Image).Source = "img_circle.png";
                    }
                }
            }
        }

        private void Handle_OnError(object sender, EventArgs e)
        {
            Content.AbortAnimation("shake");
            Content.Animate("shake",
                            (arg) =>
                            {
                                var shift = Math.Sin(2 * 2 * Math.PI * arg);
                                Content.TranslationX = 6 * shift;
                            },
                            16 * 4,
                            250,
                            Easing.Linear,
                            (arg1, arg2) =>
                            {
                                Content.TranslationX = 0;
                            });
        }
    }
}

