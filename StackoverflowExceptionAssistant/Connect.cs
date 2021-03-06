//using System;
//using Extensibility;
//using EnvDTE;
//using EnvDTE80;
//using Microsoft.VisualStudio.CommandBars;
//using System.Resources;
//using System.Reflection;
//using System.Globalization;

//namespace JeremyThompsonLabs.StackoverflowExceptionAssistant
//{
//    /// <summary>The object for implementing an Add-in.</summary>
//    /// <seealso class='IDTExtensibility2' />
//    public class Connect : IDTExtensibility2, IDTCommandTarget
//    {
//        private EnvDTE.DTE _applicationObject;
//        private AddIn _addInInstance;
//        private DebuggerEvents _debuggerEvents = null;
//        private LastException _lastException = null;

//        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
//        public Connect()
//        {
//        }

//        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
//        /// <param term='application'>Root object of the host application.</param>
//        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
//        /// <param term='addInInst'>Object representing this Add-in.</param>
//        /// <seealso class='IDTExtensibility2' />
//        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
//        {
//            System.Diagnostics.Debugger.Launch();
//            _applicationObject = (EnvDTE.DTE)application;
//            _addInInstance = (AddIn)addInInst;

//            if (connectMode == ext_ConnectMode.ext_cm_UISetup)
//            {
//                try
//                {
//                    Globals globals = null;
//                    globals = _applicationObject.Solution.Globals;
//                    _debuggerEvents = globals.DTE.Events.DebuggerEvents;
//                    _debuggerEvents.OnEnterBreakMode += new _dispDebuggerEvents_OnEnterBreakModeEventHandler(_debuggerEvents_OnEnterBreakMode);
//                    _debuggerEvents.OnExceptionThrown += new _dispDebuggerEvents_OnExceptionThrownEventHandler(_debuggerEvents_OnExceptionThrown);
//                    _debuggerEvents.OnExceptionNotHandled += new _dispDebuggerEvents_OnExceptionNotHandledEventHandler(_debuggerEvents_OnExceptionNotHandled);
//                }
//                catch (System.ArgumentException)
//                {
//                    //If we are here, then the exception is probably because a command with that name
//                    //  already exists. If so there is no need to recreate the command and we can 
//                    //  safely ignore the exception.
//                }
//            }
//        }

//        private void _debuggerEvents_OnExceptionNotHandled(string ExceptionType, string Name, int Code, string Description, ref dbgExceptionAction ExceptionAction)
//        {
//            System.Diagnostics.Debug.WriteLine("NsdfdsotHandled\n");
//            System.Diagnostics.Debug.WriteLine("Debug\n");
//        }

//        private void _debuggerEvents_OnExceptionThrown(string ExceptionType, string Name, int Code, string Description, ref dbgExceptionAction ExceptionAction)
//        {
//            System.Diagnostics.Debug.WriteLine("Thrown\n");
//        }

//        private void _debuggerEvents_OnEnterBreakMode(dbgEventReason reason, ref dbgExecutionAction execAction)
//        {
//            //System.Windows.Forms.MessageBox.Show("Debugger enters break mode. " +
//            //                                   "Reason: " + reason.ToString());
//            EnvDTE.Debugger debugger = _applicationObject.Debugger;
//            Expression exp = debugger.GetExpression("&g_my_global_foo,d");
//        }


//        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
//        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
//        /// <param term='custom'>Array of parameters that are host application specific.</param>
//        /// <seealso class='IDTExtensibility2' />
//        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
//        {
//        }

//        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
//        /// <param term='custom'>Array of parameters that are host application specific.</param>
//        /// <seealso class='IDTExtensibility2' />		
//        public void OnAddInsUpdate(ref Array custom)
//        {
//        }

//        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
//        /// <param term='custom'>Array of parameters that are host application specific.</param>
//        /// <seealso class='IDTExtensibility2' />
//        public void OnStartupComplete(ref Array custom)
//        {
//        }

//        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
//        /// <param term='custom'>Array of parameters that are host application specific.</param>
//        /// <seealso class='IDTExtensibility2' />
//        public void OnBeginShutdown(ref Array custom)
//        {
//        }

//        /// <summary>Implements the QueryStatus method of the IDTCommandTarget interface. This is called when the command's availability is updated</summary>
//        /// <param term='commandName'>The name of the command to determine state for.</param>
//        /// <param term='neededText'>Text that is needed for the command.</param>
//        /// <param term='status'>The state of the command in the user interface.</param>
//        /// <param term='commandText'>Text requested by the neededText parameter.</param>
//        /// <seealso class='Exec' />
//        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
//        {
//            if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
//            {
//                if (commandName == "StackoverflowExceptionAssistant.Connect.StackoverflowExceptionAssistant")
//                {
//                    status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
//                    return;
//                }
//            }
//        }

//        /// <summary>Implements the Exec method of the IDTCommandTarget interface. This is called when the command is invoked.</summary>
//        /// <param term='commandName'>The name of the command to execute.</param>
//        /// <param term='executeOption'>Describes how the command should be run.</param>
//        /// <param term='varIn'>Parameters passed from the caller to the command handler.</param>
//        /// <param term='varOut'>Parameters passed from the command handler to the caller.</param>
//        /// <param term='handled'>Informs the caller if the command was handled or not.</param>
//        /// <seealso class='Exec' />
//        public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
//        {
//            handled = false;
//            if (executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
//            {
//                if (commandName == "StackoverflowExceptionAssistant.Connect.StackoverflowExceptionAssistant")
//                {
//                    handled = true;
//                    return;
//                }
//            }
//        }

//    }
//}