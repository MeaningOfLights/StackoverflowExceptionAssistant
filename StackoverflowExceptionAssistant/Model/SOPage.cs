using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeremyThompsonLabs.StackoverflowExceptionAssistant
{
    public class SOPage
    {
        internal string Title { get; set; }
        internal string URL { get; set; }
        internal string Snippet { get; set; }
        internal int Answers { get; set; }
        internal long QuestionersUserId { get; set; }
        internal int QuestionUpvotes { get; set; }
        internal long[] AnswerersUserId { get; set; }
        internal int[] AnswerUpvotes { get; set; }
        internal bool[] AnswerersUserIdsInTop100InTag { get; set; }
        internal bool[] AnswerersAreBountyHuntersInTop100 { get; set; }
        internal bool HasBounty { get; set; }
        internal bool DidHaveBounty { get; set; } //For future use

        //public SOPage(string title, string url, int answers, long questionersUserId, int questionUpvotes, long[] answerersUserId, int[] answerUpvotes, bool[] answerersUserIdsInTop100InTag, bool[] answerersAreBountyHuntersInTop100, bool hasBounty)
        //{
        //    Title = title;
        //    URL = url;
        //    Answers = answers;
        //    QuestionersUserId = questionersUserId;
        //    QuestionUpvotes = questionUpvotes;
        //    AnswerersUserId = answerersUserId;
        //    AnswerUpvotes = answerUpvotes;
        //    AnswerersUserIdsInTop100InTag = answerersUserIdsInTop100InTag;
        //    AnswerersAreBountyHuntersInTop100 = answerersAreBountyHuntersInTop100;
        //    HasBounty = hasBounty;
        //}
        
    }
}
