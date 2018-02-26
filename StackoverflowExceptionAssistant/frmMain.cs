using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.Win32;

namespace JeremyThompsonLabs.StackoverflowExceptionAssistant
{
    public partial class frmMain : Form
    {
        #region Member Variables

        private DTE _dte;
        private jsonConfig _jsonConfig = new jsonConfig();
        private LastException _lastException = new LastException();
        private List<string> _cachedPages = new List<string>();
        public List<SOPage> _stackoverflowResults;

        frmHoverQA _frmQA;

        private const int maxCachePages = 5;

        #endregion

        public frmMain()
        {
            InitializeComponent();

            //SetWebBrowserToIE10();
            //SaveConfiguration();
            if (!LoadConfiguration()) return;
        }

        public frmMain(DTE dte, LastException lastException) : this()
        {
            _dte = dte;
            _lastException = lastException;
            _stackoverflowResults = SearchQueryBasedOnExceptionDescription();

            //Perform the lookup
            //_lookupResults = GoogleCustomSearch();

            //Look up the QA's scanning the pages for Info (Q: Votes, A: Votes, Accepted, Top User, Has Bounty/Top Bounty Hunter
            //DownloadHelper dh = new DownloadHelper();

            webBrowser1.ScriptErrorsSuppressed = true;
            if (_stackoverflowResults?.Count > 0) webBrowser1.Navigate(_stackoverflowResults[0].URL);
            PopulateGrid();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            _stackoverflowResults = GoogleStackoverflowSearch(textBox1.Text);
            PopulateGrid();
        }

        private void PopulateGrid()
        {
            Random rnd = new Random();
            DataTable dt = new DataTable();
            dt.Columns.Add("Source");
            dt.Columns.Add("Title");
            dt.Columns.Add("AcceptedAnswer", typeof(bool));
            dt.Columns.Add("NoOfAnswer");
            dt.Columns.Add("Votes");
            dt.Columns.Add("TopUsers");
            foreach (var QAPage in _stackoverflowResults)
            {
                DataRow dr = dt.NewRow();
                Uri url = new Uri(QAPage.URL);
                string baseUrl = url.GetComponents(UriComponents.Host, UriFormat.Unescaped);
                dr[0] = baseUrl;
                dr[1] = QAPage.Title;
                dr[2] = true;
                dr[3] = Convert.ToInt32((rnd.NextDouble() * 10));
                dr[4] = Convert.ToInt32((rnd.NextDouble() * 5));
                dr[5] = Convert.ToInt32((rnd.NextDouble() * 2));
                dt.Rows.Add(dr);
            }
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[1].Width = 600;
        }

        private void dataGridView1_MouseLeave(object sender, EventArgs e)
        {
            //TODO Trying to set webBrowser1 with Focus so that users dont have to click webBrowser1 to scroll, this doesn't work in some environments
            webBrowser1.Focus();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            webBrowser1.Navigate(_stackoverflowResults[e.RowIndex].URL);
        }

        public List<SOPage> SearchQueryBasedOnExceptionDescription()
        {
            string query = SymbolicSearch.GetSymbolicSearch(_dte, _lastException.Description, _jsonConfig);
            textBox1.Text = query;
            return GoogleStackoverflowSearch(query);
        }

        public List<SOPage> GoogleStackoverflowSearch(string query)
        {
            const string apiKey = "AIzaSyA04T2CMJSaGS9AWl66v43rzZTi7z4iKJw";
            const string searchEngineId = "015600743573451307836:fzhqm3defkg";

            CustomsearchService customSearchService = new CustomsearchService(new Google.Apis.Services.BaseClientService.Initializer() { ApiKey = apiKey });
            Google.Apis.Customsearch.v1.CseResource.ListRequest listRequest = customSearchService.Cse.List(query);
            listRequest.Cx = searchEngineId;
            Search search = listRequest.Execute();
            
            List<SOPage> results = new List<SOPage>();
            if  (search?.Items?.Count > 0) foreach (var item in search.Items) results.Add(new SOPage{ Title = item.Title, URL = item.Link });
            return results;
        }
        
        private void frmMain_Load(object sender, EventArgs e)
        {

        }
        
        private bool LoadConfiguration()
        {
            string configJSONFile = System.IO.Path.Combine(Application.StartupPath.Replace("\\bin", string.Empty).Replace("\\Debug", string.Empty).Replace("\\Release", string.Empty).Replace("\\x86", string.Empty).Replace("TestClient", "StackoverflowExceptionAssistant"), "Configuration.json");
            if (!File.Exists(configJSONFile))
            {
                MessageBox.Show("The Stackoverflow Exception Assistant Configuration.json file does not exist in the Applications Startup Folder: " + Application.StartupPath + Environment.NewLine + Environment.NewLine + "Stackoverflow Exception Assistant will now quit.", "Cannot find config file...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            try
            {
                string jsonConfigFileContents = FileHelper.ReadFileTextWithEncoding(configJSONFile);
                _jsonConfig = fastJSON.JSON.ToObject<jsonConfig>(jsonConfigFileContents);
            }
            catch (Exception ex)
            {
                MessageBox.Show("The Configuration.json file failed to load, Exception: " + ex.Message + Environment.NewLine + Environment.NewLine + "Stackoverflow Exception Assistant  will now quit.", "Failed to load config file...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Environment.Exit(2);
            }
            return true;
        }

        private bool SaveConfiguration()
        {
            string configJSONFile = System.IO.Path.Combine(Application.StartupPath.Replace("\\bin\\Debug", "").Replace("TestClient", "StackoverflowExceptionAssistant"), "Configuration.json");
 
            jsonConfig c = new jsonConfig();
            c.searchEngineWebsite = "015600743573451307836:fzhqm3defkg";
            c.cultureInfo = "en-US";
            c.opacity = 60;

            c.resultSort = ResultsSort.Relevance;
            c.answersSort = AnswersSort.Votes;

            c.topVBUserIds = "17034,22656,284240,3043,23283,1197518,1968,23354,34397,1359668,1583,65358,366904,1070452,69083,29407,27528,719186,500974,87698,88656,33708,105570,142637,48910,897326,479512,1002323,44597,702199,93623,82187,15639,14149,34211,231316,306651,320,584183,15498,1246391,1348647,2330053,1842065,328193,257954,59111,142822,880990,19307,273200,66532,707111,52249,1163867,1967056,61974,76337,240733,2688,1115360,492405,491243,73070,49241,13302,12950,130611,16076,98713,19299,144424,468973,256431,11683,2480047,84651,8521,1248295,44389,437768,341762,60761,22970,71059,13990,961113,483179,250832,310574,69527,3740093,13279,505088,259769,60188,117870,60682,1081897,1228";
            c.topCsharpUserIds = "22656,23354,88656,17034,29407,34397,65358,23283,1583,284240,69083,33708,263693,1159478,60761,76217,61974,98713,961113,335858,55847,470005,45914,13302,3043,34211,267,413501,572644,329769,93623,84651,50079,1081897,1197518,12950,82187,1968,41071,414076,122718,615,1163867,13627,412770,44389,15861,932418,126014,76337,366904,505088,3010968,993547,13552,447156,1715579,152602,445517,1870803,13087,91671,16076,59303,270591,477420,80274,15541,5380,264697,5445,67,105570,38206,613130,106159,400547,56778,885318,87698,1053,266143,156695,40347,14357,8155,2319407,73070,1228,141172,2530848,60188,1965,103167,63756,24874,38360,314488,11830,15498";
            c.topCppUserIds = "922184,34509,596781,179910,204847,151292,415784,673730,560648,661519,87234,33213,14065,36565,187690,147192,13005,140719,103167,1968,452307,12711,734069,649665,440558,19563,1932150,85371,1120273,298661,2069064,576911,335858,464581,252000,1774667,5987,129570,1782465,981959,17034,46642,500104,1919155,14860,893,501557,1708801,150634,15416,153285,251738,2756719,505088,2877241,9530,440119,3153,224671,241536,23283,123111,434551,410767,2684539,88656,28169,567292,241631,367273,469935,597607,57428,1413395,65863,168225,365496,496161,721269,1033896,15168,14089,214671,2380830,56338,3647361,82320,1011995,13430,165520,726361,20984,481267,819272,4342498,1498580,1593860,636019,166749,841108";

            c.topBountyHunterIds = "17034, 6309, 157882, 403671, 2558882, 315935, 771848, 1212341, 493939, 29407, 1025118, 115145, 100297, 22656, 1237411, 2083078, 495455, 1906307, 1085891, 1881610, 847363, 517815, 2415822, 149573, 2334192, 5311735, 833622, 4653, 23354, 82788, 5202563, 1184641, 367456, 3155639, 1528401, 1161878, 20860, 983912, 1945960, 1226894, 2646526, 1395668, 872395, 2293534, 1693593, 1045444, 70604, 326480, 857361, 2898867, 291788, 737790, 369707";

            c.excludeProjectName = true;
            c.excludeSolutionName = true;

            c.codingLanguage = "";

            c.noiseWords = "about,after,all,also,an,and,another,any,are,as,at,be,because,been,before,being,between,both,but,by,came,can,come,could,did,do,each,for,from,get,got,has,had,he,have,her,here,him,himself,his,how,if,in,into,is,it,like,make,many,me,might,more,most,much,must,my,never,now,of,on,only,or,other,our,out,over,said,same,see,should,since,some,still,such,take,than,that,the,their,them,then,there,these,they,this,those,through,to,too,under,up,very,was,way,we,well,were,what,where,which,while,who,with,would,you,your,a".Split(',');
            c.replaceNoiseWordsWithAsterisk = true;

            c.upvoteCopyPastedQuestion = true;
            c.upvoteCopyPastedAnswer = true;

            string jsonSettings1 = fastJSON.JSON.ToJSON(c);
            File.WriteAllText(configJSONFile, jsonSettings1);

            return true;
        }

     private void SetWebBrowserToIE10()
        {
            string executablePath = Environment.GetCommandLineArgs()[0];
            string executableName = System.IO.Path.GetFileName(executablePath);
          

            RegistryKey registrybrowser = Registry.CurrentUser.OpenSubKey
               (@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);

            if (registrybrowser == null)
            {
                RegistryKey registryFolder = Registry.CurrentUser.OpenSubKey
                    (@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl", true);
                registrybrowser = registryFolder.CreateSubKey("FEATURE_BROWSER_EMULATION");
            }
            registrybrowser.SetValue("TestClient.exe", 0x02710, RegistryValueKind.DWord);
            registrybrowser.Close();
        }

    }
}
