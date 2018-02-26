using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeremyThompsonLabs.StackoverflowExceptionAssistant
{
    public class jsonConfig
    {
        public const string googleSite = "site:";
        public string searchEngineWebsite { get; set; } //  defaults to "Stackoverflow.com";
        public string cultureInfo { get; set; } //  defaults to "en-US";
        public int opacity { get; set; }

        public ResultsSort resultSort { get; set; }
        public AnswersSort answersSort { get; set; }

        public string topVBUserIds { get; set; }
        public string topCsharpUserIds { get; set; }
        public string topCppUserIds { get; set; }
        public string topBountyHunterIds { get; set; }

        public bool excludeProjectName { get; set; }
        public bool excludeSolutionName { get; set; }

        public string codingLanguage { get; set; }

        public string[] noiseWords { get; set; }
        public bool replaceNoiseWordsWithAsterisk { get; set; }

        public bool upvoteCopyPastedQuestion { get; set; }
        public bool upvoteCopyPastedAnswer { get; set; }
    }

    public enum ResultsSort
    {
        Relevance,
        Votes,
        HighRepUsers,
        MostViewed,
        Bounty
    }
    
    public enum AnswersSort
    {
        Votes,
        Active,
        Oldest
    }    
}
