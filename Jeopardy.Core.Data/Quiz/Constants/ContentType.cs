using Jeopardy.Core.Localization.Locales;
using System.ComponentModel.DataAnnotations;

namespace Jeopardy.Core.Data.Quiz.Constants
{
    public enum ContentType
    {
        [Display(Description = "Enum_Undefined", ResourceType = typeof(LocaleCommon))]
        Undefined = 0,
        //Simple text to show
        [Display(Description = "ContentType_Text", ResourceType = typeof(LocaleCommon))]
        Text = 1,
        //An image to show
        [Display(Description = "ContentType_Image", ResourceType = typeof(LocaleCommon))]
        Image = 2,
        //Sound to playback
        [Display(Description = "ContentType_Sound", ResourceType = typeof(LocaleCommon))]
        Sound = 3,
        //Video to playback
        [Display(Description = "ContentType_Video", ResourceType = typeof(LocaleCommon))]
        Video = 4
    };
}
