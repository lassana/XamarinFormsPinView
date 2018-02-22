using System;
using Xamarin.Forms;
using Xunit;

namespace FormsPinView.PCL.Tests
{
    /// <summary>
    /// PinItemView tests.
    /// </summary>
    public class PinItemViewTests
    {
        /// <summary>
        /// Tests the Text property: if it's being set and if the corresponding 
        /// PropertyChanged event is being raised.
        /// </summary>
        [Fact]
        public void TestTextProperty()
        {
            var view = new PinItemView();
            string expectedValue = Guid.NewGuid().ToString();
            Assert.PropertyChanged(view, 
                                   PinItemView.TextProperty.PropertyName, 
                                   () => view.Text = expectedValue);
            Assert.Same(expectedValue, view.Text);
        }

        /// <summary>
        /// Tests the Command property: if it's being set and if the 
        /// corresponding PropertyChanged event is being raised.
        /// </summary>
        [Fact]
        public void TestCommandProperty()
        {
            var view = new PinItemView();
            Command expectedValue = new Command((obj) => {});
            Assert.PropertyChanged(view,
                                   PinItemView.CommandProperty.PropertyName,
                                   () => view.Command = expectedValue);
            Assert.Same(expectedValue, view.Command);
        }

        /// <summary>
        /// Tests the CommandParameter property: if it's being set and if the 
        /// corresponding PropertyChanged event is being raised.
        /// </summary>
        [Fact]
        public void TestCommandParameterProperty()
        {
            var view = new PinItemView();
            string expectedValue = Guid.NewGuid().ToString();
            Assert.PropertyChanged(view,
                                   PinItemView.CommandParameterProperty.PropertyName,
                                   () => view.CommandParameter = expectedValue);
            Assert.Same(expectedValue, view.CommandParameter);
        }
    }
}
