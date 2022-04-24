using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Jeopardy.Core.Localization.Extensions
{
    public static class EnumLocalizationExtensions
    {
        public static string GetDisplayDescription(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .FirstOrDefault()?
                            .GetCustomAttribute<DisplayAttribute>()?
                            .GetDescription() ?? "unknown";
        }
    }
}
