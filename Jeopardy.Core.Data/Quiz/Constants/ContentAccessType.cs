using Jeopardy.Core.Localization.Locales;
using System.ComponentModel.DataAnnotations;

namespace Jeopardy.Core.Data.Quiz.Constants
{
    public enum ContentAccessType
    {
        [Display(Description = "Enum_Undefined", ResourceType = typeof(LocaleCommon))]
        Undefined = 0,
        //Access via embedded binary
        [Display(Description = "ContentAccessType_Embedded", ResourceType = typeof(LocaleCommon))]
        Embedded = 1,
        //Access via link on the internet
        [Display(Description = "ContentAccessType_Link", ResourceType = typeof(LocaleCommon))]
        Link = 2
    }
}
