using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xamarin.Forms;

namespace FormsPinView.PCL.Tests
{
    /// <summary>
    /// PinView tests.
    /// </summary>
    public class PinViewTests
    {
        public PinViewTests()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
        }

        /// <summary>
        /// Tests if a valid PIN is handled correctly.
        /// </summary>
        [Fact]
        public void TestCorrectPin()
        {
            var targetPin = new[] { '1', '2', '3', '4' };
            var viewModel = new PinViewModel
            {
                TargetPinLength = 4,
                ValidatorFunc = (IList<char> arg) => Enumerable.SequenceEqual(targetPin, arg)
            };
            var view = new PinView
            {
                BindingContext = viewModel
            };
            Func<string, PinItemView> findButton = (string arg) => 
            {
                return ((PinItemView)((Grid)view.Content).Children.First(c => c is PinItemView btn && btn.Text == arg));
            };
            Assert.Raises<EventArgs>(
                (EventHandler<EventArgs> obj) => viewModel.Success += obj,
                (EventHandler<EventArgs> obj) => viewModel.Success -= obj,
                () => 
                {
                    foreach (char next in targetPin)
                    {
                        PinItemView btn = findButton(next.ToString());
                        btn.Command.Execute(btn.CommandParameter);
                    }
                }
            );
        }

        /// <summary>
        /// Tests if an invalid PIN is handled correctly.
        /// </summary>
        [Fact]
        public void TestincorrectPin()
        {
            var targetPin = new[] { '1', '2', '3', '4' };
            var viewModel = new PinViewModel
            {
                TargetPinLength = 4,
                ValidatorFunc = (IList<char> arg) => Enumerable.SequenceEqual(targetPin, arg)
            };
            var view = new PinView
            {
                BindingContext = viewModel
            };
            Func<string, PinItemView> findButton = (string arg) =>
            {
                return ((PinItemView)((Grid)view.Content).Children.First(c => c is PinItemView btn && btn.Text == arg));
            };
            Assert.Raises<EventArgs>(
                (EventHandler<EventArgs> obj) => viewModel.Error += obj,
                (EventHandler<EventArgs> obj) => viewModel.Error -= obj,
                () =>
                {
                foreach (char next in targetPin.Reverse())
                    {
                        PinItemView btn = findButton(next.ToString());
                        btn.Command.Execute(btn.CommandParameter);
                    }
                }
            );
        }
    }
}
