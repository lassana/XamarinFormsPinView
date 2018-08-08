using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xunit;

namespace FormsPinView.Core.Tests
{
    /// <summary>
    /// <see cref="PinView"/> tests.
    /// </summary>
    public class PinViewTests
    {
        public PinViewTests()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
        }

        private PinItemView FindButton(PinView view, string text)
        {
            return ((PinItemView)(view).Children.First(c => c is PinItemView btn && btn.Text == text));
        }

        /// <summary>
        /// Tests if a valid PIN is handled correctly.
        /// </summary>
        [Fact]
        public void TestCorrectPin()
        {
            var targetPin = new[] { '1', '2', '3', '4' };
            var view = new PinView
            {
                PinLength = 4,
                Validator = (IList<char> arg) => Enumerable.SequenceEqual(targetPin, arg)
            };
            Assert.Raises<EventArgs>(
                (EventHandler<EventArgs> obj) => view.Success += obj,
                (EventHandler<EventArgs> obj) => view.Success -= obj,
                () => 
                {
                    foreach (char next in targetPin)
                    {
                        PinItemView btn = FindButton(view, next.ToString());
                        btn.Command.Execute(btn.CommandParameter);
                    }
                }
            );
        }

        /// <summary>
        /// Tests if all PIN symbols is filled after a valid PIN is entered.
        /// </summary>
        [Fact]
        public void TestCorrectPin_AllSymbolsFilled()
        {
            var targetPin = new[] { '1', '2', '3', '4' };
            var view = new PinView
            {
                PinLength = 4,
                Validator = (IList<char> arg) => Enumerable.SequenceEqual(targetPin, arg),
                ClearAfterSuccess = false
            };
            foreach (char next in targetPin)
            {
                PinItemView btn = FindButton(view, next.ToString());
                btn.Command.Execute(btn.CommandParameter);
            }

            var sl = (StackLayout)(view).Children.First(c => c is StackLayout);
            foreach (View child in sl.Children)
            {
                var cv = (CircleView)child;
                Assert.NotNull(cv);
                Assert.True(cv.IsFilledUp);
            }
        }


        /// <summary>
        /// Tests if a <code>SuccessCommand</code> is handled correctly.
        /// </summary>
        [Fact]
        public void TestSuccessCommand()
        {
            bool invoked = false;
            var targetPin = new[] { '1', '2', '3', '4' };
            var view = new PinView
            {
                PinLength = 4,
                Validator = (IList<char> arg) => Enumerable.SequenceEqual(targetPin, arg),
                SuccessCommand = new Command(() => invoked = true)
            };
            foreach (char next in targetPin)
            {
                PinItemView btn = FindButton(view, next.ToString());
                btn.Command.Execute(btn.CommandParameter);
            }
            Assert.True(invoked);
        }

        /// <summary>
        /// Tests if an invalid PIN is handled correctly.
        /// </summary>
        [Fact]
        public void TestIncorrectPin()
        {
            var targetPin = new[] { '1', '2', '3', '4' };
            var view = new PinView
            {
                PinLength = 4,
                Validator = (IList<char> arg) => Enumerable.SequenceEqual(targetPin, arg)
            };
            PinItemView findButton(string arg)
            {
                return ((PinItemView)(view).Children.First(c => c is PinItemView btn && btn.Text == arg));
            }
            Assert.Raises<EventArgs>(
                (EventHandler<EventArgs> obj) => view.Error += obj,
                (EventHandler<EventArgs> obj) => view.Error -= obj,
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


        /// <summary>
        /// Tests if a <code>ErrorCommand</code> is handled correctly.
        /// </summary>
        [Fact]
        public void TestErrorCommand()
        {
            bool invoked = false;
            var targetPin = new[] { '1', '2', '3', '4' };
            var view = new PinView
            {
                PinLength = 4,
                Validator = (IList<char> arg) => Enumerable.SequenceEqual(targetPin, arg),
                ErrorCommand = new Command(() => invoked = true)
            };
            foreach (char next in targetPin.Reverse())
            {
                PinItemView btn = FindButton(view, next.ToString());
                btn.Command.Execute(btn.CommandParameter);
            }
            Assert.True(invoked);
        }

        /// <summary>
        /// Tests the PinLength property: if it's being set and 
        /// if the corresponding PropertyChanged event is being raised.
        /// </summary>
        [Fact]
        public void TestPinLengthPropertyChangedRaised()
        {
            var pinView = new PinView();
            int expectedValue = 3;
            Assert.PropertyChanged(pinView,
                                   nameof(PinView.PinLength),
                                   () => pinView.PinLength = expectedValue);
            Assert.Equal(expectedValue, pinView.PinLength);
        }

        /// <summary>
        /// Tests the PinLength cannot be set to non-positive value.
        /// </summary>
        [Fact]
        public void TestNobPositivePinLengthException()
        {
            var pinView = new PinView();
            Assert.Throws<ArgumentException>(() => { pinView.PinLength = -2; });
            Assert.Throws<ArgumentException>(() => { pinView.PinLength = 0; });
        }

        /// <summary>
        /// Tests the Validator property: if it's being set and 
        /// if the corresponding PropertyChanged event is being raised.
        /// </summary>
        [Fact]
        public void TestValidatorPropertyChangedRaised()
        {
            var PinView = new PinView();
            var expectedValue = new Func<IList<char>, bool>((arg) => false);
            Assert.PropertyChanged(PinView,
                                   nameof(PinView.Validator),
                                   () => PinView.Validator = expectedValue);
            Assert.Same(expectedValue, PinView.Validator);
        }

        /// <summary>
        /// Tests the EnteredPin property: if it's being set and 
        /// if the corresponding PropertyChanged event is being raised.
        /// </summary>
        [Fact]
        public void TestEnteredPinPropertyChangedRaised()
        {
            var PinView = new PinView();
            var expectedValue = new List<char> { 'a', 'b' };
            Assert.PropertyChanged(PinView,
                                   nameof(PinView.EnteredPin),
                                   () => PinView.EnteredPin = expectedValue);
            Assert.Same(expectedValue, PinView.EnteredPin);
        }

        /// <summary>
        /// Tests if the DisplayedTextUpdated is being raised correctly and 
        /// if the EnteredPin is being updated correctly.
        /// </summary>
        [Fact]
        public void TestFewSymboldEntered()
        {
            var pinView = new PinView
            {
                PinLength = 5,
                Validator = new Func<IList<char>, bool>((arg) => false)
            };
            Assert.Raises(
                (EventHandler<EventArgs> obj) => { pinView.DisplayedTextUpdated += obj; },
                (EventHandler<EventArgs> obj) => { pinView.DisplayedTextUpdated -= obj; },
                () =>
                {
                    pinView.KeyPressedCommand.Execute("1");
                    pinView.KeyPressedCommand.Execute("1");
                }
            );
            Assert.True(Enumerable.SequenceEqual(new[] { '1', '1' }, pinView.EnteredPin));
        }

        /// <summary>
        /// Tests if the "Backspace" key event is being handled correctly.
        /// </summary>
        [Fact]
        public void TestBackspacePressed()
        {
            var pinView = new PinView
            {
                PinLength = 5,
                Validator = new Func<IList<char>, bool>((arg) => false)
            };
            pinView.KeyPressedCommand.Execute("1");
            pinView.KeyPressedCommand.Execute("1");
            pinView.KeyPressedCommand.Execute("2");
            pinView.KeyPressedCommand.Execute("Backspace");
            Assert.True(Enumerable.SequenceEqual(new[] { '1', '1' }, pinView.EnteredPin));
        }

        /// <summary>
        /// Tests if an invalid PIN is being handled correctly.
        /// </summary>
        [Fact]
        public void TestErrorRaised()
        {
            var pinView = new PinView
            {
                PinLength = 5,
                Validator = new Func<IList<char>, bool>((arg) => false)
            };
            pinView.KeyPressedCommand.Execute("1");
            pinView.KeyPressedCommand.Execute("2");
            pinView.KeyPressedCommand.Execute("3");
            pinView.KeyPressedCommand.Execute("4");
            Assert.Raises(
                (EventHandler<EventArgs> obj) => { pinView.Error += obj; },
                (EventHandler<EventArgs> obj) => { pinView.Error -= obj; },
                () => { pinView.KeyPressedCommand.Execute("5"); }
            );
            Assert.Empty(pinView.EnteredPin);
        }

        /// <summary>
        /// Tests if an valid PIN is being handled correctly.
        /// </summary>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestDisplayedTextUpdatedRaised(bool clearAfterSuccess)
        {
            var pinView = new PinView
            {
                PinLength = 5,
                ClearAfterSuccess = clearAfterSuccess
            };
            pinView.Validator = new Func<IList<char>, bool>(
                (arg) => Enumerable.SequenceEqual(new[] { '1', '2', '3', '4', '5' }, pinView.EnteredPin)
            );
            pinView.KeyPressedCommand.Execute("1");
            pinView.KeyPressedCommand.Execute("2");
            pinView.KeyPressedCommand.Execute("3");
            pinView.KeyPressedCommand.Execute("4");
            Assert.Raises(
                (EventHandler<EventArgs> obj) => { pinView.Success += obj; },
                (EventHandler<EventArgs> obj) => { pinView.Success -= obj; },
                () => { pinView.KeyPressedCommand.Execute("5"); }
            );

            if (clearAfterSuccess)
                Assert.Empty(pinView.EnteredPin);
            else
                Assert.NotEmpty(pinView.EnteredPin);
        }

        /// <summary>
        /// Tests if color of all children is being changed when setting the view color.
        /// </summary>
        [Fact]
        public void TestColorChange()
        {
            var pinView = new PinView();
            var expectedValue = Color.GreenYellow;
            pinView.Color = expectedValue;

            Assert.Equal(expectedValue, pinView.Color);

            var statusLayout = (StackLayout)pinView.Children[0];
            foreach (View child in statusLayout.Children)
            {
                Assert.Equal(expectedValue, ((CircleView)child).Color);
            }
            foreach(View view in pinView.Children)
            {
                if (view is CircleView circleView) 
                if (view is PinItemView pinItemView) Assert.Equal(expectedValue, pinItemView.Color);
            }
        }

        /// <summary>
        /// Tests if tint color of all children is being changed when setting the view tint color.
        /// </summary>
        [Fact]
        public void TestBorderColorChange()
        {
            var pinView = new PinView();
            var expectedValue = Color.Violet;
            pinView.BorderColor = expectedValue;

            Assert.Equal(expectedValue, pinView.BorderColor);
            foreach (View view in pinView.Children)
            {
                if (view is PinItemView pinItemView) Assert.Equal(expectedValue, pinItemView.BorderColor);
            }
        }

        /// <summary>
        /// Tests if ripple color of all children is being changed when setting the view tint color.
        /// </summary>
        [Fact]
        public void TestRippleColorChange()
        {
            var pinView = new PinView();
            var expectedValue = Color.Red;
            pinView.RippleColor = expectedValue;

            Assert.Equal(expectedValue, pinView.RippleColor);
            foreach (View view in pinView.Children)
            {
                if (view is PinItemView pinItemView) Assert.Equal(expectedValue, pinItemView.RippleColor);
            }
        }
    }
}