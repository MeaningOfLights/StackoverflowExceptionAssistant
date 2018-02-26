using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeremyThompsonLabs.StackoverflowExceptionAssistant
{
    class CompilerErrorHelper
    {
        public CompilerErrorHelper()
        {
            //http://www.bing.com/search?q=CS0246++C%23+The+type+or+namespace+name+could+not+be+found+(are+you+missing+a+using+directive+or+an+assembly+reference%3f)&form=VSHELP
            //https://bingdev.cloudapp.net/BingUrl.svc/Get?selectedText=%3B%20expected&mainLanguage=C%23&projectType=%7B82b43b9b-a64c-4715-b499-d71e9ca2bd60%7D%3B%7B60dc8134-eba5-43b8-bcc9-bb4bc16c2548%7D%3B%7BFAE04EC0-301F-11D3-BF4B-00C04F79EFBC%7D&requestId=5fe
            //http://www.bing.com/search?q=CS0246++C%23+The+type+or+namespace+name+could+not+be+found+(are+you+missing+a+using+directive+or+an+assembly+reference%3f)&form=VSHELP
            //https://bingdev.cloudapp.net/BingUrl.svc/Get?selectedText=The%20name%20%27AnswersSort%27%20does%20not%20exist%20in%20the%20current%20context&mainLanguage=C%23&projectType=%7BFAE04EC0-301F-11D3-BF4B-00C04F79EFBC%7D&requestId=36056ead-20e4-455b-9370-4b08a7606ff7&clientId=&errorCode=CS0103

            //int i = 1 / 0;
            //https://support.microsoft.com/en-us/kb/304656
        }
    }
}
