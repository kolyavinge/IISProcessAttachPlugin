using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Linq;

namespace IISProcessAttach.Model
{
    public class IISProcess
    {
        private readonly string _iisProcessName = "w3wp.exe";
        private readonly AsyncPackage _package;

        public IISProcess(AsyncPackage package)
        {
            _package = package;
        }

        public void Attach()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var dte = (DTE)Package.GetGlobalService(typeof(DTE));
            var processes = dte.Debugger.LocalProcesses;
            var iisProcessList = processes.Cast<Process>().Where(proc => proc.Name.IndexOf(_iisProcessName) != -1).ToList();
            if (iisProcessList.Any())
            {
                foreach (var iisProcess in iisProcessList)
                {
                    iisProcess.Attach();
                }
            }
            else
            {
                VsShellUtilities.ShowMessageBox(
                    _package,
                    "No IIS process found          ",
                    "",
                    OLEMSGICON.OLEMSGICON_INFO,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }
        }
    }
}
