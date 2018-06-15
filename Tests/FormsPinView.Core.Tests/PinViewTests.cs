using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xunit;

namespace FormsPinView.Core.Tests
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
            var view = new PinView
            {
                TargetPinLength = 4,
                Validator = (IList<char> arg) => Enumerable.SequenceEqual(targetPin, arg)
            };
            Func<string, PinItemView> findButton = (string arg) => 
            {
                return ((PinItemView)((Grid)view).Children.First(c => c is PinItemView btn && btn.Text == arg));
            };
            Assert.Raises<EventArgs>(
                (EventHandler<EventArgs> obj) => view.Success += obj,
                (EventHandler<EventArgs> obj) => view.Success -= obj,
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
            var view = new PinView
            {
                TargetPinLength = 4,
                Validator = (IList<char> arg) => Enumerable.SequenceEqual(targetPin, arg)
            };
            Func<string, PinItemView> findButton = (string arg) =>
            {
                return ((PinItemView)((Grid)view).Children.First(c => c is PinItemView btn && btn.Text == arg));
            };
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
        /// Tests the TargetPinLength property: if it's being set and 
        /// if the corresponding PropertyChanged event is being raised.
        /// </summary>
        [Fact]
        public void TestTargetPinLengthPropertyChangedRaised()
        {
            var pinView = new PinView();
            int expectedValue = 3;
            Assert.PropertyChanged(pinView,
                                   nameof(PinView.TargetPinLength),
                                   () => pinView.TargetPinLength = expectedValue);
            Assert.Equal(expectedValue, pinView.TargetPinLength);
        }

        /// <summary>
        /// Tests the TargetPinLength cannot be set to non-positive value.
        /// </summary>
        [Fact]
        public void TestNobPositiveTargetPinLengthException()
        {
            var pinView = new PinView();
            Assert.Throws<ArgumentException>(() => { pinView.TargetPinLength = -2; });
            Assert.Throws<ArgumentException>(() => { pinView.TargetPinLength = 0; });
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
            var pinView = new PinView();
            pinView.TargetPinLength = 5;
            pinView.Validator = new Func<IList<char>, bool>((arg) => false);
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
            var pinView = new PinView();
            pinView.TargetPinLength = 5;
            pinView.Validator = new Func<IList<char>, bool>((arg) => false);
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
            var pinView = new PinView();
            pinView.TargetPinLength = 5;
            pinView.Validator = new Func<IList<char>, bool>((arg) => false);
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
        [Fact]
        public void TestDisplayedTextUpdatedRaised()
        {
            var pinView = new PinView();
            pinView.TargetPinLength = 5;
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
            Assert.Empty(pinView.EnteredPin);
        }
    }
}