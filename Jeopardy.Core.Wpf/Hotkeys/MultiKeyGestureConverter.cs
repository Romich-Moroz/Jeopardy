using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

namespace Jeopardy.Core.Wpf.Hotkeys
{
    public class MultiKeyGestureConverter : TypeConverter
    {
        private readonly KeyConverter _keyConverter = new();
        private readonly ModifierKeysConverter _modifierKeysConverter = new();

        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) => sourceType == typeof(string);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string gesture)
            {
                var allKeys = gesture.Split('+');
                IEnumerable<string>? modifierKeys = allKeys.SkipLast(1);
                var keyStrokes = allKeys.Last().Split(',');

                ModifierKeys modifiers = modifierKeys.Select(modifierKeyString => (ModifierKeys?)_modifierKeysConverter.ConvertFrom(modifierKeyString) ?? ModifierKeys.None).
                    Where(modifierKey => modifierKey != ModifierKeys.None).
                    Aggregate((modifierKey1, modifierKey2) => modifierKey1 | modifierKey2);

                IEnumerable<Key>? keys = keyStrokes.Select(keyStroke => (Key?)_keyConverter.ConvertFrom(keyStroke) ?? Key.None).
                    Where(keyStroke => keyStroke != Key.None);

                return new MultiKeyGesture(keys, modifiers);
            }

            throw new ArgumentException($"Could not convert {nameof(MultiKeyGesture)} from argument type {value.GetType()} of {nameof(value)}");
        }
    }
}
