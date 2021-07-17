using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace TimeTracker
{
    public static class ProcessTracker
    {
        private static ProcessUpdater ProcessUpdater;
        private static Point InitialPoint = new Point(20, 20);
        private static TrackingProcess currentActiveProcess;
        private static ControlCollection panelControls;

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public static void SetupTracking(Panel mainPanel)
        {
            if(!(mainPanel is null))
            {
                panelControls = mainPanel.Controls;
            }
            else
            {
                throw new Exception("No panel");
            }
            //Start tracking process
            TrackProcess();
        }

        private static void TrackProcess()
        {
            ProcessUpdater = new ProcessUpdater();
            var activeProcess = GetActiveWindowTitle();

            if (!ProcessUpdater.ContainsProcesses())
            {
                AddProcessToList(activeProcess);
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

            /*if (currentActiveProcess.ProcessName == currentWindow)
            {
                //keep adding time to total usage
                if (!(currentActiveProcess is null))
                {
                    currentActiveProcess.UpdateTime();
                    var val = currentActiveProcess.GetValue();
                }
            }
            else
            {
                currentActiveProcess.StopStopwatch();
                var process = Processes.FirstOrDefault(proc => currentWindow.Contains(proc.ProcessName));
                //switch process to new active process and begin counting time
                currentActiveProcess = process;
                currentActiveProcess.StartStopwatch();
                if (!(process is null))
                {
                    process.UpdateTime();
                    var val = process.GetValue();
                }
            }*/
        }

        private static void AddProcessToList(string ProcessName, bool newProcess = false)
        {
            if (newProcess)
            {
                InitialPoint = new Point(InitialPoint.X, InitialPoint.Y + ProcessUpdater.LastAddedProcess().GetProcessBarHeight() + 5);
                var process = new TrackingProcess(ProcessName, panelControls, InitialPoint);
                ProcessUpdater.Add(process);
                currentActiveProcess = process;
            }
            else
            {
                var process = new TrackingProcess(ProcessName, panelControls, InitialPoint);
                ProcessUpdater.Add(process);
                currentActiveProcess = process;
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
