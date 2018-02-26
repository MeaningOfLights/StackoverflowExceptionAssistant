using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JeremyThompsonLabs.StackoverflowExceptionAssistant
{
    public partial class frmHoverQA : Form
    {
        public frmHoverQA(string link)
        {
            InitializeComponent();

            //webBrowser1.DocumentCompleted += OnWebBrowserDocumentCompleted;
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate(link); // "http://stackoverflow.com/questions/40518220/cefsharp-web-browser-with-zscaler-and-f5-doesnt-cache-sso-credentials");

            //Setting the Document Text doesn't load CSS or Javascript
            //webBrowser1.DocumentText = htmlContent;
        }

        private void frmHoverQA_Load(object sender, EventArgs e)
        {
      
        }

        private void OnWebBrowserDocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Document.Window.Error += OnWebBrowserDocumentWindowError;
        }

        private void OnWebBrowserDocumentWindowError(object sender, System.Windows.Forms.HtmlElementErrorEventArgs e)
        {
            //Suppresses a dialog and continues running scripts on the page
            e.Handled = true;
        }
    }
}
