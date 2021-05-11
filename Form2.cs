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

        private List<string> knownProcesses = new List<string>
        {
            "Skype", "Discord", "Battle.net", "steam", "mspaint", "paint"
        };

        public List<TrackingProcess> Processes { get; private set; }

        private Point InitialPoint = new Point(20, 20);

        private bool rendered = false;

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

        private void button1_Click(object sender, EventArgs e)
        {
            var allProcesses = Process.GetProcesses();
            var localAll = allProcesses.Where(process => knownProcesses.Any(process2 => process2.Equals(process.ProcessName)));
            foreach (var process in localAll)
            {
                if (Processes is null)
                {
                    Processes = new List<TrackingProcess>
                    {
                        new TrackingProcess(process, panel1.Controls, InitialPoint)
                    };
                }
                else
                {
                    var proc = Processes.ElementAt(Processes.Count() - 1);

                    InitialPoint = new Point(InitialPoint.X, InitialPoint.Y + proc.GetProcessBarHeight() + 5);
                    Processes.Add(new TrackingProcess(process, panel1.Controls, InitialPoint));
                }
            }
        }

        public void UpdateProcesses()
        {
            var currentWindow = GetActiveWindowTitle();
            Debug.WriteLine(currentWindow);
            var tmp = Processes.FirstOrDefault(proc => currentWindow.Contains(proc.Process.ProcessName));
            if (!(tmp is null))
            {
                var val = tmp.GetValue();
                tmp.SetValue(++val);
            }
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

            if (!rendered)
            {
                button1_Click(sender, e);
                rendered = true;
            }
            else
            {
                UpdateProcesses();
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
