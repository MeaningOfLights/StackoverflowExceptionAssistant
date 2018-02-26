using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeremyThompsonLabs.StackoverflowExceptionAssistant
{
    static class GuidList
    {
        public const string guidStackoverflowExceptionAssistantPkgString = "E0EB2F47-667E-4262-B700-5A14BD80FA17";
        public const string guidStackoverflowExceptionAssistantCmdSetString = "60D6DAC3-7732-420E-807E-C3B1B0B28000";

        public static readonly Guid guidStackoverflowExceptionAssistantCmdSet = new Guid(guidStackoverflowExceptionAssistantCmdSetString);
    };
}
