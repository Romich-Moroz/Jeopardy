using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Jeopardy.Core.Wpf.Hotkeys
{
    public class MultiKeyBinding : InputBinding
    {
        [TypeConverter(typeof(MultiKeyGestureConverter))]
        public override InputGesture Gesture
        {
            get => base.Gesture;
            set
            {
                if (value is not MultiKeyGesture)
                {
                    throw new ArgumentException($"Could not convert {nameof(MultiKeyGesture)} from argument type {value.GetType()} of {nameof(value)}");
                }

                base.Gesture = value;
            }
        }
    }
}
