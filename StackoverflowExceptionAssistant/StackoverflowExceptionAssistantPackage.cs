using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
//using Microsoft.Win32;
using Microsoft.VisualStudio;
//using Microsoft.VisualStudio.Shell.Interop;
//using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
//using System.Threading;
//using System.Collections;
//using System.Collections.Generic;
using System.Linq;
using Thread = EnvDTE.Thread;
using EnvDTE;
using System.IO;

namespace JeremyThompsonLabs.StackoverflowExceptionAssistant
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#113", "#114", "1.0", IconResourceID = 300)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionOpening_string)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidStackoverflowExceptionAssistantPkgString)]
    public sealed class StackoverflowExceptionAssistantPackage : Package
    {
        #region Member Variables

        private DebuggerEvents _debuggerEvents = null;
        private LastException _lastException = null;
        private EnvDTE.DTE dte;

        OleMenuCommand StandardExceptionAssistantCmd;
        OleMenuCommand StackoverflowExceptionAssistantCmd;

        bool IsInStackoverflowExceptionAssistantMode = false;

        #endregion

        #region Ctor and Initialise

        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not initialised inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public StackoverflowExceptionAssistantPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this));
        }


       
        /// <summary>
        /// Initialization of the package; this method is called right after the package is instantiated, 
        /// put all the initilaization code that rely on services provided by VisualStudio here.
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this));
            base.Initialize();

            this.dte = (EnvDTE.DTE)GetGlobalService(typeof(EnvDTE.DTE));

            //Hook up the events to intercept the Exceptions
            Globals globals = null;
            globals = dte.Solution.Globals;
            _debuggerEvents = dte.Events.DebuggerEvents;
            _debuggerEvents.OnEnterBreakMode += new _dispDebuggerEvents_OnEnterBreakModeEventHandler(OnEnterBreakMode);
            _debuggerEvents.OnExceptionThrown += new _dispDebuggerEvents_OnExceptionThrownEventHandler(OnExceptionThrown);
            _debuggerEvents.OnExceptionNotHandled += new _dispDebuggerEvents_OnExceptionNotHandledEventHandler(OnExceptionNotHandled);

            //Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the menu item.
                var menuCommandID = new CommandID(GuidList.guidStackoverflowExceptionAssistantCmdSet, (int)PkgCmdIDList.StandardExceptionAssistant);
                var menuCommandID2 = new CommandID(GuidList.guidStackoverflowExceptionAssistantCmdSet, (int)PkgCmdIDList.StackoverflowExceptionAssistant);
                this.StandardExceptionAssistantCmd = new OleMenuCommand(StandardExceptionAssistant, menuCommandID);
                this.StackoverflowExceptionAssistantCmd = new OleMenuCommand(StackoverflowExceptionAssistant, menuCommandID2);
                this.StandardExceptionAssistantCmd.BeforeQueryStatus += OnBeforeQueryStatus;
                this.StackoverflowExceptionAssistantCmd.BeforeQueryStatus += OnBeforeQueryStatus;

                mcs.AddCommand(this.StandardExceptionAssistantCmd);
                mcs.AddCommand(this.StackoverflowExceptionAssistantCmd);
            }
            this.dte = (EnvDTE.DTE)GetGlobalService(typeof(EnvDTE.DTE));
            this.OnBeforeQueryStatus(this.StandardExceptionAssistantCmd, null);
            this.OnBeforeQueryStatus(this.StackoverflowExceptionAssistantCmd, null);
        }

        //protected OptionPageGrid Settings
        //{
        //    get { return (OptionPageGrid)GetDialogPage(typeof(OptionPageGrid)); }
        //}

        #endregion

        #region Exception Events

        private void OnExceptionNotHandled(string ExceptionType, string Name, int Code, string Description, ref dbgExceptionAction ExceptionAction)
        {
            Description = "{\"<user xmlns=''> was not expected.}";
            //I'm gonna give the dev dude who named the arguments in parameter with uppercase a spanking
            _lastException = new LastException { ExceptionType = ExceptionType, Name = Name, Code = Code, Description = Description, ExceptionAction = ExceptionAction };
            System.Diagnostics.Debug.WriteLine("NsdfdsotHandled\n");
            System.Diagnostics.Debug.WriteLine("Debug\n");
        }

        private void OnExceptionThrown(string ExceptionType, string Name, int Code, string Description, ref dbgExceptionAction ExceptionAction)
        {
            System.Diagnostics.Debug.WriteLine("Thrown\n");
        }

        private void OnEnterBreakMode(dbgEventReason reason, ref dbgExecutionAction execAction)
        {
            Debug.WriteLine("got here at: " + DateTime.Now.ToShortTimeString());
            if (reason == EnvDTE.dbgEventReason.dbgEventReasonExceptionNotHandled)
            {
                MainWindow frm = new MainWindow(dte, _lastException);
                frm.ShowDialog();
                //frmMain frm = new frmMain(dte, _lastException);
                //frm.ShowDialog();
            }
        }

        #endregion

        #region Toolbar Buttons - turn off and on Stackoverflow or standard Exception Assistants

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void StandardExceptionAssistant(object sender, EventArgs e)
        {
            this.dte = (EnvDTE.DTE)GetGlobalService(typeof(EnvDTE.DTE));
            dte.Properties["Debugging", "General"].Item("EnableExceptionAssistant").Value = false;
        }

        private void StackoverflowExceptionAssistant(object sender, EventArgs e)
        {
            this.dte = (EnvDTE.DTE)GetGlobalService(typeof(EnvDTE.DTE));
            dte.Properties["Debugging", "General"].Item("EnableExceptionAssistant").Value = true;
        }

        private void OnBeforeQueryStatus(object sender, EventArgs e)
        {
            var myCommand = sender as OleMenuCommand;
            if (null != myCommand)
            {
                this.dte = (EnvDTE.DTE)GetGlobalService(typeof(EnvDTE.DTE));
                this.IsInStackoverflowExceptionAssistantMode = !Convert.ToBoolean(dte.Properties["Debugging", "General"].Item("EnableExceptionAssistant").Value);

                myCommand.Enabled = true;
                myCommand.Visible = true;

                if (myCommand.CommandID == this.StackoverflowExceptionAssistantCmd.CommandID)
                {
                    if (this.IsInStackoverflowExceptionAssistantMode)
                    {
                        myCommand.Checked = true;
                        this.StandardExceptionAssistantCmd.Checked = false;
                    }
                }
                else if(myCommand.CommandID == this.StandardExceptionAssistantCmd.CommandID)
                {
                    if (!this.IsInStackoverflowExceptionAssistantMode)
                    {
                        myCommand.Checked = true;
                        this.StackoverflowExceptionAssistantCmd.Checked = false;
                    }
                }
            }
        }

        #endregion
    }
}
