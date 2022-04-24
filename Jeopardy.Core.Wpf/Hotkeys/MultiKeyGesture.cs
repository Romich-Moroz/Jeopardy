using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Jeopardy.Core.Wpf.Hotkeys
{
    [TypeConverter(typeof(MultiKeyGestureConverter))]
    public class MultiKeyGesture : KeyGesture
    {
        private static readonly TimeSpan _maximumDelayBetweenKeyPresses = TimeSpan.FromSeconds(1);

        private readonly IList<Key> _keys;

        private DateTime _lastKeyPressDateTime;
        private int _currentKeyIndex;

        public MultiKeyGesture(IEnumerable<Key> keys, ModifierKeys modifiers)
            : this(keys, modifiers, string.Empty)
        {
        }

        public MultiKeyGesture(IEnumerable<Key> keys, ModifierKeys modifiers, string displayString)
            : base(Key.None, modifiers, displayString)
        {
            if (!keys.Any())
            {
                throw new ArgumentException("At least one key must be specified.", nameof(keys));
            }

            _keys = new List<Key>(keys);
        }

        public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
        {
            if (inputEventArgs is not KeyEventArgs args ||
                !IsDefinedKey(args.Key) ||
                IsExpiredTimeFrame() ||
                IsInvalidModifiers() ||
                IsWrongKey(args.Key))
            {
                _currentKeyIndex = 0;
                return false;
            }

            _currentKeyIndex++;

            if (_currentKeyIndex < _keys.Count)
            {
                _lastKeyPressDateTime = DateTime.Now;
                inputEventArgs.Handled = true;
                return false;
            }

            _currentKeyIndex = 0;
            return true;
        }

        private static bool IsDefinedKey(Key key)
        {
            return key >= Key.None && key <= Key.OemClear;
        }

        private bool IsExpiredTimeFrame()
        {
            return _currentKeyIndex != 0 && !(DateTime.Now - _lastKeyPressDateTime <= _maximumDelayBetweenKeyPresses);
        }

        private bool IsInvalidModifiers()
        {
            return _currentKeyIndex == 0 && Modifiers != Keyboard.Modifiers;
        }

        private bool IsWrongKey(Key key)
        {
            return _keys[_currentKeyIndex] != key;
        }
    }
}
