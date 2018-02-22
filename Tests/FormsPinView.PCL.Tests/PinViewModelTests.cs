using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace FormsPinView.PCL.Tests
{
    /// <summary>
    /// PinViewModel tests.
    /// </summary>
    public class PinViewModelTests
    {
        /// <summary>
        /// Tests the TargetPinLength property: if it's being set and 
        /// if the corresponding PropertyChanged event is being raised.
        /// </summary>
        [Fact]
        public void TestTargetPinLengthPropertyChangedRaised()
        {
            var pinViewModel = new PinViewModel();
            int expectedValue = 3;
            Assert.PropertyChanged(pinViewModel,
                                   nameof(PinViewModel.TargetPinLength),
                                   () => pinViewModel.TargetPinLength = expectedValue);
            Assert.Equal(expectedValue, pinViewModel.TargetPinLength);
        }

        /// <summary>
        /// Tests the TargetPinLength cannot be set to non-positive value.
        /// </summary>
        [Fact]
        public void TestNobPositiveTargetPinLengthException()
        {
            var pinViewModel = new PinViewModel();
            Assert.Throws<ArgumentException>(() => { pinViewModel.TargetPinLength = -2; });
            Assert.Throws<ArgumentException>(() => { pinViewModel.TargetPinLength = 0; });
        }

        /// <summary>
        /// Tests the ValidatorFunc property: if it's being set and 
        /// if the corresponding PropertyChanged event is being raised.
        /// </summary>
        [Fact]
        public void TestValidatorFuncPropertyChangedRaised()
        {
            var pinViewModel = new PinViewModel();
            var expectedValue = new Func<IList<char>, bool>((arg) => false);
            Assert.PropertyChanged(pinViewModel,
                                   nameof(PinViewModel.ValidatorFunc),
                                   () => pinViewModel.ValidatorFunc = expectedValue);
            Assert.Same(expectedValue, pinViewModel.ValidatorFunc);
        }

        /// <summary>
        /// Tests the EnteredPin property: if it's being set and 
        /// if the corresponding PropertyChanged event is being raised.
        /// </summary>
        [Fact]
        public void TestEnteredPinPropertyChangedRaised()
        {
            var pinViewModel = new PinViewModel();
            var expectedValue = new List<char> { 'a', 'b' };
            Assert.PropertyChanged(pinViewModel,
                                   nameof(PinViewModel.EnteredPin),
                                   () => pinViewModel.EnteredPin = expectedValue);
            Assert.Same(expectedValue, pinViewModel.EnteredPin);
        }

        /// <summary>
        /// Tests if the DisplayedTextUpdated is being raised correctly and 
        /// if the EnteredPin is being updated correctly.
        /// </summary>
        [Fact]
        public void TestFewSymboldEntered()
        {
            var pinViewModel = new PinViewModel();
            pinViewModel.TargetPinLength = 5;
            pinViewModel.ValidatorFunc = new Func<IList<char>, bool>((arg) => false);
            Assert.Raises(
                (EventHandler<EventArgs> obj) => { pinViewModel.DisplayedTextUpdated += obj; },
                (EventHandler<EventArgs> obj) => { pinViewModel.DisplayedTextUpdated -= obj; },
                () => 
                {
                    pinViewModel.KeyPressCommand.Execute("1");
                    pinViewModel.KeyPressCommand.Execute("1");
                }
            );
            Assert.True(Enumerable.SequenceEqual(new []{'1','1'}, pinViewModel.EnteredPin));
        }

        /// <summary>
        /// Tests if the "Backspace" key event is being handled correctly.
        /// </summary>
        [Fact]
        public void TestBackspacePressed()
        {
            var pinViewModel = new PinViewModel();
            pinViewModel.TargetPinLength = 5;
            pinViewModel.ValidatorFunc = new Func<IList<char>, bool>((arg) => false);
            pinViewModel.KeyPressCommand.Execute("1");
            pinViewModel.KeyPressCommand.Execute("1");
            pinViewModel.KeyPressCommand.Execute("2");
            pinViewModel.KeyPressCommand.Execute("Backspace");
            Assert.True(Enumerable.SequenceEqual(new[] { '1', '1' }, pinViewModel.EnteredPin));
        }

        /// <summary>
        /// Tests if an invalid PIN is being handled correctly.
        /// </summary>
        [Fact]
        public void TestErrorRaised()
        {
            var pinViewModel = new PinViewModel();
            pinViewModel.TargetPinLength = 5;
            pinViewModel.ValidatorFunc = new Func<IList<char>, bool>((arg) => false);
            pinViewModel.KeyPressCommand.Execute("1");
            pinViewModel.KeyPressCommand.Execute("2");
            pinViewModel.KeyPressCommand.Execute("3");
            pinViewModel.KeyPressCommand.Execute("4");
            Assert.Raises(
                (EventHandler<EventArgs> obj) => { pinViewModel.Error += obj; },
                (EventHandler<EventArgs> obj) => { pinViewModel.Error -= obj; },
                () => { pinViewModel.KeyPressCommand.Execute("5"); }
            );
            Assert.Empty(pinViewModel.EnteredPin);
        }

        /// <summary>
        /// Tests if an valid PIN is being handled correctly.
        /// </summary>
        [Fact]
        public void TestDisplayedTextUpdatedRaised()
        {
            var pinViewModel = new PinViewModel();
            pinViewModel.TargetPinLength = 5;
            pinViewModel.ValidatorFunc = new Func<IList<char>, bool>(
                (arg) => Enumerable.SequenceEqual(new[] { '1', '2', '3', '4', '5' }, pinViewModel.EnteredPin)
            );
            pinViewModel.KeyPressCommand.Execute("1");
            pinViewModel.KeyPressCommand.Execute("2");
            pinViewModel.KeyPressCommand.Execute("3");
            pinViewModel.KeyPressCommand.Execute("4");
            Assert.Raises(
                (EventHandler<EventArgs> obj) => { pinViewModel.Success += obj; },
                (EventHandler<EventArgs> obj) => { pinViewModel.Success -= obj; },
                () => { pinViewModel.KeyPressCommand.Execute("5"); }
            );
            Assert.Empty(pinViewModel.EnteredPin);
        }
    }
}