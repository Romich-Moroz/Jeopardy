using Jeopardy.Core.Localization.Extensions;
using System;
using System.Linq;
using System.Windows.Markup;

namespace Jeopardy.Core.Wpf.MarkupExtensions
{
    public class EnumerationExtension : MarkupExtension
    {
        private Type _enumType;

        public EnumerationExtension(Type enumType)
        {
            _enumType = enumType ?? throw new ArgumentNullException(nameof(enumType));
        }

        public Type EnumType
        {
            get => _enumType;
            private set
            {
                if (_enumType == value)
                {
                    return;
                }

                Type? enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (enumType.IsEnum == false)
                {
                    throw new ArgumentException("Type must be an Enum.");
                }

                _enumType = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(EnumType)
                       .Cast<Enum>()
                       .Skip(1)
                       .Select(v => new EnumerationMember(v.GetDisplayDescription(), v))
                       .ToArray();
        }

        public class EnumerationMember
        {
            public string Description { get; set; }
            public object Value { get; set; }

            public EnumerationMember(string description, object value)
            {
                (Description, Value) = (description, value);
            }
        }
    }
}
