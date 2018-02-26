using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeremyThompsonLabs.StackoverflowExceptionAssistant
{
    public static class SearchHelper
    {
        public static List<SOPage> SearchQueryBasedOnExceptionDescription(DTE dte, LastException lastException)
        {
            string query = SymbolicSearch.GetSymbolicSearch(dte, lastException.Description, null);// _jsonConfig);
            return GoogleStackoverflowAndMSDNSearch(query);
        }

        public static string GetSymbolicSearch(DTE dte, string searchQuery, jsonConfig jsonConfig)
        {
            //Remove additional information
            int indexOfAdditionalInfo = searchQuery.ToString().IndexOf("Additional information");
            if (indexOfAdditionalInfo > 0) searchQuery = searchQuery.Substring(0, indexOfAdditionalInfo);

            StringBuilder sbSearchQuery = new StringBuilder(searchQuery);
            
            //Remove noise words
            //string wildCard = jsonConfig.replaceNoiseWordsWithAsterisk ? "*" : string.Empty;
            //foreach (string word in jsonConfig.noiseWords) sbSearchQuery.Replace(" " + word + " ", " " + wildCard + " ");
            //sbSearchQuery.Replace(" " + wildCard + " " + wildCard + " ", " " + wildCard + " ");

            //Remove Solution name
            sbSearchQuery.Replace(dte?.Solution?.FullName, string.Empty);

            //Remove Application name
            foreach (string word in GetProjectNames(dte)) sbSearchQuery.Replace(word, string.Empty);

            sbSearchQuery.Replace("\n", string.Empty);

            //Inject language(s)
            sbSearchQuery.Insert(0, (string.IsNullOrEmpty(jsonConfig?.codingLanguage) ? GetCodeLanguage(dte) : jsonConfig.codingLanguage));
            
            return sbSearchQuery.ToString();
        }

        private static string GetCodeLanguage(DTE dte)
        {
            if (dte == null) return string.Empty;
            if (dte.Solution == null) return string.Empty;

            string langs = "";
            foreach (Project proj in dte.Solution)
            {
                try
                {
                    CodeModel cm = proj.CodeModel;
                   // switch (cm.Language)
                    //{
                    //    case CodeModelLanguageConstants.vsCMLanguageMC:
                    //    case CodeModelLanguageConstants.vsCMLanguageVC:
                    //        langs += "C++ ";
                    //        break;
                    //    case CodeModelLanguageConstants.vsCMLanguageCSharp:
                    //        langs += "C# ";
                    //        break;
                    //    case CodeModelLanguageConstants.vsCMLanguageVB:
                    //        langs += "VB.Net ";
                    //        break;
                    //    case "{E6FDF8BF-F3D1-11D4-8576-0002A516ECE8}":
                    //        langs += "J# ";
                    //        break;
                    //}
                }
                catch
                {
                }
            }
            return langs;
        }

        private static List<string> GetProjectNames(DTE dte)
        {
            if (dte == null) return new List<string>();
            if (dte.Solution == null) return new List<string>();

            List<string> projectNames = new List<string>();
            foreach (Project proj in dte.Solution)
            {
                projectNames.Add(proj.Name);
            }
            return projectNames;
        }
    
        public static  List<SOPage> GoogleStackoverflowAndMSDNSearch(string query)
        {
            const string apiKey = "AIzaSyA04T2CMJSaGS9AWl66v43rzZTi7z4iKJw";
            const string searchEngineId = "015600743573451307836:fzhqm3defkg";

            var customSearchService = new Google.Apis.Customsearch.v1.CustomsearchService(new Google.Apis.Services.BaseClientService.Initializer() { ApiKey = apiKey });
            Google.Apis.Customsearch.v1.CseResource.ListRequest listRequest = customSearchService.Cse.List(query);
            listRequest.Cx = searchEngineId;
            Google.Apis.Customsearch.v1.Data.Search search = listRequest.Execute();

            List<SOPage> results = new List<SOPage>();
            
            if (search?.Items?.Count > 0) foreach (var item in search.Items) results.Add(new SOPage { Title = item.Title, URL = item.Link, Snippet = item.Snippet });
            return results;
        }
    }
}
