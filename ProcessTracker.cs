using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace TimeTracker
{
    public static class ProcessTracker
    {
        private static ProcessUpdater ProcessUpdater { get; set; }
        private static Point InitialPoint = new Point(20, 20);
        private static TrackingProcess currentActiveProcess;
        private static ControlCollection panelControls;

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public static void SetupTracking(Panel mainPanel, ProcessUpdater procUpdater)
        {
            panelControls = mainPanel.Controls ?? throw new Exception("MainPanel controls are null");
            ProcessUpdater = procUpdater ?? throw new Exception("ProcessUpdater is null");
        }

        public static void TrackProcess()
        {
            var activeProcess = GetActiveWindowTitle();

            if (!ProcessUpdater.ContainsProcesses())
            {
                AddProcessToList(activeProcess,false);
            }
            else if(!ProcessUpdater.HasProcess(activeProcess))
            {
                // add a new process to the list of processes
                AddProcessToList(activeProcess, true);
            }
            else
            {
                UpdateProcesses(activeProcess);
            }
        }

        public static void UpdateProcesses(string currentWindow)
        {
            Debug.WriteLine(currentWindow);

            if(ProcessUpdater.GetCurrentActiveProcess().ProcessName != currentWindow)
            {
                currentActiveProcess = ProcessUpdater.GetProcess(currentWindow);
                ProcessUpdater.SetActiveProcess(currentActiveProcess);
                currentActiveProcess.UpdateTime();
            }
            else
            {

                currentActiveProcess = ProcessUpdater.GetCurrentActiveProcess();
                currentActiveProcess.UpdateTime();
            }
        }

        private static void AddProcessToList(string ProcessName, bool newProcess = false)
        {
            if (newProcess)
            {
                InitialPoint = new Point(InitialPoint.X, InitialPoint.Y + ProcessUpdater.LastAddedProcess().GetProcessBarHeight() + 5);
                var process = new TrackingProcess(ProcessName, panelControls, InitialPoint);
                ProcessUpdater.AddProcess(process);
            }
            else
            {
                var process = new TrackingProcess(ProcessName, panelControls, InitialPoint);
                ProcessUpdater.AddProcess(process);
            }
        }

        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
    }
}
