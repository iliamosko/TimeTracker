using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeTracker
{
    public partial class Form2 : Form
    {
        readonly DateTime start;

        public List<TrackingProcess> Processes { get; private set; }

        private Point InitialPoint = new Point(20, 20);

        private TrackingProcess currentActiveProcess;

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public Form2()
        {
            start = DateTime.UtcNow;
            
            InitializeComponent();
            panel1.AutoScroll = true;
            TimeElapsed();
        }

        /// <summary>
        /// Creates a timer that will update every second.
        /// </summary>
        private void TimeElapsed()
        {
            var timer = new Timer
            {
                Interval = 1000
            };
            timer.Tick += new EventHandler(UpdateTime);
            timer.Start();
        }

        private void UpdateTime(object sender, EventArgs e)
        {
            var currentTime = DateTime.UtcNow;
            var timeDifference = currentTime - start;
            var hours = timeDifference.Hours;
            var minutes = timeDifference.Minutes;
            var seconds = timeDifference.Seconds;
            label2.Text = $"{hours:00}:{minutes:00}:{seconds:00}";

            // TODO:
            // as well as updating the time, should add an update to the running processes
            // that updates the progress bar
            TrackProcess();
        }

        private void TrackProcess()
        {
            var activeProcess = GetActiveWindowTitle();


            if (Processes is null)
            {
                var process = new TrackingProcess(activeProcess, panel1.Controls, InitialPoint);
                Processes = new List<TrackingProcess>
                {
                    process
                };
                currentActiveProcess = process;
            }
            else if (!Processes.Exists(proc => proc.ProcessName.Equals(activeProcess)))
            {
                InitialPoint = new Point(InitialPoint.X, InitialPoint.Y + Processes.LastOrDefault().GetProcessBarHeight() + 5);
                var process = new TrackingProcess(activeProcess, panel1.Controls, InitialPoint);
                Processes.Add(process);
                currentActiveProcess = process;
            }
            else
            {
                UpdateProcesses(activeProcess);
            }
        }

        public void UpdateProcesses(string currentWindow)
        {
            Debug.WriteLine(currentWindow);

            if(currentActiveProcess.ProcessName == currentWindow)
            {
                //keep adding time to total usage
                var process = Processes.FirstOrDefault(proc => currentWindow.Contains(proc.ProcessName));
                if (!(process is null))
                {
                    process.UpdateTime();
                    var val = process.GetValue();
                    process.SetValue(--val);
                }
            }
            else
            {
                currentActiveProcess.StopStopwatch();
                var process = Processes.FirstOrDefault(proc => currentWindow.Contains(proc.ProcessName));
                //switch process to new active process and beging counting time
                currentActiveProcess = process;
                currentActiveProcess.StartStopwatch();
                if (!(process is null))
                {
                    process.UpdateTime();
                    var val = process.GetValue();
                    process.SetValue(--val);
                }
            }
           
        }

        private string GetActiveWindowTitle()
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
