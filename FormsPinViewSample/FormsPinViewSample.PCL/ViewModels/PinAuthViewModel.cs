using FormsPinView.PCL;
using System.Linq;

namespace FormsPinViewSample.ViewModels
{
    public class PinAuthViewModel
    {
        private static readonly char[] s_pin = new[] { '1', '2', '3', '4' };

        public PinViewModel PinViewModel { get; private set; } = new PinViewModel
        {
            TargetPinLength = 4,
            ValidatorFunc = (arg) => 
            {
                return arg.SequenceEqual(s_pin);
            }
        };
    }
}

