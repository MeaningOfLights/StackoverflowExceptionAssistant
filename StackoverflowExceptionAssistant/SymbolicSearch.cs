using System;
using EnvDTE;
using System.Text;
using System.Collections.Generic;

namespace JeremyThompsonLabs.StackoverflowExceptionAssistant
{
    public static class SymbolicSearch
    {
        public static string GetSymbolicSearch(DTE dte, string searchQuery, jsonConfig jsonConfig)
        {
            //Remove additional information
            int indexOfAdditionalInfo = searchQuery.ToString().IndexOf("Additional information");
            if (indexOfAdditionalInfo > 0) searchQuery = searchQuery.Substring(0, indexOfAdditionalInfo);

            StringBuilder sbSearchQuery = new StringBuilder(searchQuery);

            //Remove Application name
            //sbSearchQuery.Replace(dte.Solution.FullName, string.Empty);

            //Remove noise words
            //string wildCard = jsonConfig.replaceNoiseWordsWithAsterisk ? "*" : string.Empty;
            //foreach (string word in jsonConfig.noiseWords) sbSearchQuery.Replace(" " + word + " ", " " + wildCard + " ");
            //sbSearchQuery.Replace(" " + wildCard + " " + wildCard + " ", " " + wildCard + " ");

            foreach (string word in GetProjectNames(dte)) sbSearchQuery.Replace(word, string.Empty);

            sbSearchQuery.Replace("\n", string.Empty);

            //Inject language(s)
            sbSearchQuery.Insert(0,(string.IsNullOrEmpty(jsonConfig?.codingLanguage) ? GetCodeLanguage(dte) : jsonConfig.codingLanguage));
            
            
            return sbSearchQuery.ToString();
        }
 
        private static string GetCodeLanguage(DTE dte)
        {
            if (dte == null) return string.Empty;
            if (dte.Solution == null) return string.Empty;

            // Before running this example, open a solution that contains one 
            // or more projects.
            string langs = "";
            foreach (Project proj in dte.Solution)
            {
                
                try
                {
                    CodeModel cm = proj.CodeModel;
                    switch (cm.Language)
                    {
                        //case CodeModelLanguageConstants.vsCMLanguageMC:
                        //case CodeModelLanguageConstants.vsCMLanguageVC:
                        //    langs += "C++ ";
                        //    break;
                        //case CodeModelLanguageConstants.vsCMLanguageCSharp:
                        //    langs += "C# ";
                        //    break;
                        //case CodeModelLanguageConstants.vsCMLanguageVB:
                        //    langs += "VB.Net ";
                        //    break;
                        //case "{E6FDF8BF-F3D1-11D4-8576-0002A516ECE8}":
                        //    langs += "J# ";
                        //    break;
                    }
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
    }
}
