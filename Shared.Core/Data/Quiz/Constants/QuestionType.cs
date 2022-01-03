namespace Shared.Core.Data.Quiz.Constants
{
    public enum QuestionType
    {
        //First to click receives the question
        Simple = 1,
        //Highest bidder receives the question
        Auction = 2,
        //Gifted person receives the question
        Cat = 3,
        //Double the prize and no point loss on wrong answer
        Sponsored = 4
    };
}
