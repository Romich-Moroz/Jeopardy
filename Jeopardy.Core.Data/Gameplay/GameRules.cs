using ProtoBuf;

namespace Jeopardy.Core.Data.Gameplay
{
    [ProtoContract]
    public class GameRules
    {
        //public const int MaxSecretQuestionRewardMultiplier = 10;
        //public const int MinSecretQuestionRewardMultiplier = 1;
        //public const int MaxStakeQuestionMaxStakeMultiplier = 10;
        //public const int MinStakeQuestionMaxStakeMultiplier = 1;
        public const int MaxQuestionAnswerTime = 60;
        public const int MinQuestionAnswerTime = 5;
        public const int MaxQuestionHangingTime = 60;
        public const int MinQuestionHangingTime = 5;

        //[ProtoMember(1)]
        //public int SecretQuestionRewardMultiplier { get; set; } = 2;
        //[ProtoMember(2)]
        //public int StakeQuestionMaxStakeMultiplier { get; set; } = 3;
        [ProtoMember(3)]
        public int QuestionAnswerTime { get; set; } = 30;
        [ProtoMember(4)]
        public int QuestionHangingTime { get; set; } = 60;
    }
}
