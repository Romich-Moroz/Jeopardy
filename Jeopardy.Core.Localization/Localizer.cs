using System.Globalization;

namespace Jeopardy.Core.Localization
{
    public static class Localizer
    {
        public static SupportedLocale CurrentLocale { get; }

        private static readonly IReadOnlyDictionary<SupportedLocale, CultureInfo> _cultures = new Dictionary<SupportedLocale, CultureInfo>
        {
            { SupportedLocale.Undefined, CultureInfo.CreateSpecificCulture("en-US") },
            { SupportedLocale.English, CultureInfo.CreateSpecificCulture("en-US") },
            { SupportedLocale.Russian, CultureInfo.CreateSpecificCulture("ru-RU") }
        };

        public static void SetCurrentLocale(SupportedLocale locale)
        {
            CultureInfo cultureInfo = _cultures[locale];

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        }
    }
}
