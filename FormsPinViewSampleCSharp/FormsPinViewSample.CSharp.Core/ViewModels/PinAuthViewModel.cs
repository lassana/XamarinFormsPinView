using System;
using System.Collections.Generic;
using System.Linq;

namespace FormsPinViewSample.Core.ViewModels
{
    public class PinAuthViewModel : ViewModelBase
    {
        private readonly char[] _correctPin;
        private readonly Func<IList<char>, bool> _validator;

        public Func<IList<char>, bool> ValidatorFunc => _validator;

        public PinAuthViewModel()
        {
            _correctPin = new[] { '1', '2', '3', '4' };
            _validator = (arg) => Enumerable.SequenceEqual(arg, _correctPin);
        }
    }
}