using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeTracker.Entities;

namespace TimeTracker.Background
{
    internal class ActiveProcessListener
    {
        Panel processNamePanel;
        Panel progressBarPanel;
        Panel timeSpentPanel;
        public ProcessManager processManager;

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public ActiveProcessListener(Panel processNamePanel, Panel progressBarPanel, Panel timeSpentPanel)
        {
            this.processNamePanel = processNamePanel;
            this.progressBarPanel = progressBarPanel;
            this.timeSpentPanel = timeSpentPanel;
            processManager = new ProcessManager();
        }

        public void TrackProcess()
        {
            var activeProcess = GetActiveWindowTitle();

            if (!processManager.ContainsProcess(activeProcess))
            {
                AddProcess(activeProcess);
            } 
            else
            {
                UpdateProcess(activeProcess);
            }
        }

        void AddProcess(string processName)
        {
            var proc = new ActiveProcess(processName, processNamePanel, progressBarPanel, timeSpentPanel);
            processManager.AddProcess(proc);
        }

        void UpdateProcess(string processName)
        {
            //look at processtracker, this needs to update the timers for the active processes
            if (!processName.Equals(processManager.GetActiveProcess()))
            {
                // change active process
                processManager.ChangeActiveProcess(processName); 
                processManager.UpdateActiveProcessBar();
            }
            else
            {
                processManager.UpdateActiveProcessBar();
            }
        }

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                //string buffString = Buff.ToString();
                //string trimmedString = String.Concat(buffString.Where(c => !Char.IsWhiteSpace(c)));
                //string[] windowTitleArray = trimmedString.Split('-');
                //string windowTitle = windowTitleArray[windowTitleArray.Length - 1] + windowTitleArray[windowTitleArray.Length - 2];

                //return windowTitle;
                return Buff.ToString();
            }
            return null;
        }
    }
}
