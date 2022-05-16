using Jeopardy.Core.Localization.Locales;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace Jeopardy.Core.Data.Quiz.Constants
{
    [ProtoContract]
    public enum QuestionType
    {
        [Display(Description = "Enum_Undefined", ResourceType = typeof(LocaleCommon))]
        Undefined = 0,
        //First to click receives the question
        [Display(Description = "QuestionType_Simple", ResourceType = typeof(LocaleCommon))]
        Simple = 1,
        ////Highest bidder receives the question
        //[Display(Description = "QuestionType_Auction", ResourceType = typeof(LocaleCommon))]
        //Auction = 2,
        ////Gifted person receives the question
        //[Display(Description = "QuestionType_Cat", ResourceType = typeof(LocaleCommon))]
        //Secret = 3,
        ////Double the prize and no point loss on wrong answer
        //[Display(Description = "QuestionType_Sponsored", ResourceType = typeof(LocaleCommon))]
        //Sponsored = 4
    };
}
