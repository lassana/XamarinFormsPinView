using Xamarin.Forms;
using Xunit;

namespace FormsPinView.Core.Tests
{
    /// <summary>
    /// <see cref="CircleView"/> tests.
    /// </summary>
    public class CircleViewTests
    {
        /// <summary>
        /// Tests the Color property: if it's being set and if the 
        /// corresponding PropertyChanged event is being raised.
        /// </summary>
        [Fact]
        public void TestColorProperty()
        {
            var view = new CircleView();
            var expectedValue = Color.Cornsilk;
            Assert.PropertyChanged(view,
                                   CircleView.ColorProperty.PropertyName,
                                   () => view.Color = expectedValue);
            Assert.Equal(expectedValue, view.Color);
        }

        /// <summary>
        /// Tests the IsFilledUp property: if it's being set and if the 
        /// corresponding PropertyChanged event is being raised.
        /// </summary>
        [Fact]
        public void IsFilledUpProperty()
        {
            var view = new CircleView();
            bool expectedValue = !(bool)CircleView.IsFilledUpProperty.DefaultValue;
            Assert.PropertyChanged(view,
                                   CircleView.IsFilledUpProperty.PropertyName,
                                   () => view.IsFilledUp = expectedValue);
            Assert.Equal(expectedValue, view.IsFilledUp);
        }
    }
}