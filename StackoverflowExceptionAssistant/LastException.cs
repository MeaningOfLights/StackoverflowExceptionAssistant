using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeremyThompsonLabs.StackoverflowExceptionAssistant
{
    public class LastException
    {
        public string ExceptionType { get; set; }
        public string Name  { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
        public EnvDTE.dbgExceptionAction ExceptionAction = 0;
    }
}
