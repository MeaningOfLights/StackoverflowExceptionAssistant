using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JeremyThompsonLabs.StackoverflowExceptionAssistant
{
    internal class DownloadHelper
    {
        private WebClient wc = new WebClient();

        internal DownloadHelper()
        {

            StringBuilder sbCookieTheme2 = new StringBuilder();
            sbCookieTheme2.Append("{");
            sbCookieTheme2.Append("\"domain\": \"stackoverflow.com\", ");
            sbCookieTheme2.Append("\"expirationDate\": 1577836781.099937, ");
            sbCookieTheme2.Append("\"hostOnly\": true, ");
            sbCookieTheme2.Append("\"httpOnly\": false, ");
            sbCookieTheme2.Append("\"name\": \"theme\", ");
            sbCookieTheme2.Append("\"path\": \" / \", ");
            sbCookieTheme2.Append("\"sameSite\": \"no_restriction\", ");
            sbCookieTheme2.Append("\"secure\": false, ");
            sbCookieTheme2.Append("\"session\": false, ");
            sbCookieTheme2.Append("\"storeId\": \"0\", ");
            sbCookieTheme2.Append("\"value\": \"2\", ");
            sbCookieTheme2.Append("\"id\": 13");
            sbCookieTheme2.Append("}");

            wc.Headers.Add(HttpRequestHeader.Cookie, sbCookieTheme2.ToString());
        }

        private byte[] TestForHTTP404(string url)
        {
            try
            {
                return wc.DownloadData(url);
            }
            catch (WebException webEx)
            {
                //Most likely a 404
            }
            catch (Exception ex)
            {
                //Something else unknown, meh.
            }
            return null;
        }

        //<div class="question"  id="question" data-questionid="40499115">
        //<div class="-summary answer" data-answerid="40499141">
        //<span itemprop="upvoteCount" class="vote-count-post mobile-vote-count">0</span>
        //<a href="/users/7097781/zhao-lin">Zhao Lin</a><br>


        public string Download(string url) //, string[] keywordsInUrl)
        {
            byte[] byteArray;
            string fileName = string.Empty;
            string fileURL = string.Empty;
            try
            {
                byteArray = wc.DownloadData(url);
                return Encoding.UTF8.GetString(byteArray);

                //foreach (LinkItem i in LinkFinder.Find(s))
                //{
                //    bool hasAllKeywordsInURL = true;
                //    for (int j = 0; j < keywordsInUrl.Length; j++)
                //    {
                //        if (i.Href == null)
                //        {
                //            hasAllKeywordsInURL = false;
                //            break;
                //        }
                //        else if (!i.Href.Contains(keywordsInUrl[j]))
                //        {
                //            hasAllKeywordsInURL = false;
                //            break;
                //        }
                //    }

                    //    if (hasAllKeywordsInURL)
                    //    {
                    //        fileURL = i.Href;
                    //        if (!fileURL.StartsWith("http://abs.gov.au")) fileURL = "http://abs.gov.au" + fileURL;
                    //        var downloadFileByteArray = wc.DownloadData(fileURL);
                    //        //F'n ABS, Content-Disposition doesn't work because the ABS dont add the HTML Meta Data to the file downloads properly - hopefully they do in the future!!
                    //        //Try to extract the filename from the Content-Disposition header
                    //        if (!String.IsNullOrEmpty(wc.ResponseHeaders["Content-Disposition"]))
                    //        {
                    //            fileName = wc.ResponseHeaders["Content-Disposition"].Substring(wc.ResponseHeaders["Content-Disposition"].IndexOf("filename=") + 10).Replace("\"", "");
                    //        }

                    //        //Resort to naming the file based on the KeyWords found in the link
                    //        if (string.IsNullOrEmpty(fileName))
                    //        {
                    //            fileName = Path.GetFileName(fileURL);
                    //            int indexOfTimeSeriesNumber = fileName.IndexOf(keywordsInUrl[0]);
                    //            int indexOfFileExtension = fileName.IndexOf(keywordsInUrl[1]);

                    //            if (indexOfTimeSeriesNumber > indexOfFileExtension)
                    //            {
                    //                //example of the anomoly: http://abs.gov.au/AUSSTATS/subscriber.nsf/log?openagent&june%20quarter%202016.xls&5519.0.55.001&Data%20Cubes&A23C4DDB023BEA6DCA25802500125385&0&June%20Quarter%202016&06.09.2016&Latest
                    //                int indexOfAmpersandBeforeFileExtension = fileName.IndexOf("&");
                    //                fileName = fileName.Substring(indexOfAmpersandBeforeFileExtension + 1, indexOfTimeSeriesNumber - indexOfAmpersandBeforeFileExtension - 2);
                    //            }
                    //            else
                    //            {
                    //                fileName = fileName.Substring(indexOfTimeSeriesNumber, indexOfFileExtension - indexOfTimeSeriesNumber + keywordsInUrl[1].Length);
                    //            }
                    //        }

                    //        string destFileName = Path.Combine(destination, fileName);
                    //        File.WriteAllBytes(destFileName, downloadFileByteArray);

                    //        AddReportFailure("00AA00", "Updated  Successfully!", fileName, destination, fileURL);
                    //        fileName = string.Empty;
                    //    }
                    //}

            }
            catch (WebException webEx)
            {
                //AddReportFailure("0000FF", "Exception: " + ((HttpWebResponse)webEx.Response).StatusCode, fileName, destination, fileURL);
                //runSheetFailures.Add("<tr><td style=\"color:#0000FF\">Exception: " + ((HttpWebResponse)webEx.Response).StatusCode + "</td><td>" + destination + " </td><td>" + fileName + " </td><td>" + fileURL + " </td></tr>");
            }
            catch (Exception Ex)
            {
                //AddReportFailure("FF0000", "Error: " + Ex.Message, fileName, destination, fileURL);
                //runSheetFailures.Add("<tr><tdstyle=\"color:#FF0000\">Error: " + Ex.Message + "</td><td>" + destination + " </td><td>" + fileName + " </td><td>" + fileURL  + "</td></tr>");
            }

            return "";
        }

    }


    public struct LinkItem
    {
        public string Href;
        public string Text;

        public override string ToString()
        {
            return Href + "\n\t" + Text;
        }
    }

    static class LinkFinder
    {
        public static List<LinkItem> Find(string file)
        {
            List<LinkItem> list = new List<LinkItem>();

            // 1.
            // Find all matches in file.
            MatchCollection m1 = Regex.Matches(file, @"(<a.*?>.*?</a>)",
                RegexOptions.Singleline);

            // 2.
            // Loop over each match.
            foreach (Match m in m1)
            {
                string value = m.Groups[1].Value;
                LinkItem i = new LinkItem();

                // 3.
                // Get href attribute.
                Match m2 = Regex.Match(value, @"href=\""(.*?)\""",
                RegexOptions.Singleline);
                if (m2.Success)
                {
                    i.Href = m2.Groups[1].Value;
                }

                // 4.
                // Remove inner tags from text.
                string t = Regex.Replace(value, @"\s*<.*?>\s*", "", RegexOptions.Singleline);
                i.Text = t;

                list.Add(i);
            }
            return list;
        }
    }
}
