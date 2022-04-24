using Jeopardy.Core.Localization.Locales;
using System.ComponentModel.DataAnnotations;

namespace Jeopardy.Core.Localization
{
    public enum SupportedLocale
    {
        [Display(Description = "Enum_Undefined", ResourceType = typeof(LocaleCommon))]
        Undefined,
        [Display(Description = "SupportedLocale_English", ResourceType = typeof(LocaleCommon))]
        English,
        [Display(Description = "SupportedLocale_Russian", ResourceType = typeof(LocaleCommon))]
        Russian
    }
}
