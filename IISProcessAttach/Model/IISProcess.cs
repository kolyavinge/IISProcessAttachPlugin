using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Linq;

namespace IISProcessAttach.Model
{
    public class IISProcess
    {
        private readonly string _iisProcessName = "w3wp.exe";

        public void Attach()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var dte = (DTE)Package.GetGlobalService(typeof(DTE));
            var iisProcessList = dte.Debugger.LocalProcesses.Cast<Process>().Where(proc => proc.Name.IndexOf(_iisProcessName) != -1).ToList();
            if (iisProcessList.Any())
            {
                foreach (var iisProcess in iisProcessList)
                {
                    try
                    {
                        iisProcess.Attach();
                        ToOutput("IIS process is successfull attached");
                    }
                    catch (Exception e)
                    {
                        ToOutput(e.ToString());
                    }
                }
            }
            else
            {
                ToOutput("IIS process was not found");
            }
        }

        private void ToOutput(string message)
        {
            var outWindow = (IVsOutputWindow)Package.GetGlobalService(typeof(SVsOutputWindow));
            Guid generalPaneGuid = VSConstants.GUID_OutWindowDebugPane;
            outWindow.GetPane(ref generalPaneGuid, out IVsOutputWindowPane generalPane);
            if (generalPane != null)
            {
                generalPane.OutputString(message + Environment.NewLine);
                generalPane.Activate();
            }
        }
    }
}
