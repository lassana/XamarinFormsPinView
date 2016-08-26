using System;
using PinView.PCL;
namespace PinViewSample.ViewModels
{
    public class PinAuthViewModel
    {
        public PinViewModel PinViewModel { get; private set; } = new PinViewModel
        {
            TargetPin = new[] { '1', '2', '3', '4' },
        };
    }
}

