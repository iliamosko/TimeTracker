using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace TimeTracker
{
    public class TrackingProcess
    {
        /// <summary>
        /// Gets the process name
        /// </summary>
        public string ProcessName { get; }

        /// <summary>
        /// Gets the process controls
        /// </summary>
        ControlCollection Controls { get; }

        /// <summary>
        /// Gets the location of the label
        /// </summary>
        Point Location { get; }

        // NOTE:
        // Stopewatch does not match timer in From2 class. Should change it to timer to match the total time.
        public Stopwatch Stopwatch { get; }

        Label ProcessLabel;
        Label TimeSpent;
        ProgressBar ProcessBar;

        /// <summary>
        /// Creates tracking information with the given process name, panel controls and the location.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <param name="controls">The controls for the process</param>
        /// <param name="location">The location of the components</param>
        public TrackingProcess(string processName, ControlCollection controls, Point location)
        {
            ProcessName = processName;
            Controls = controls;
            Location = location;
            Stopwatch = new Stopwatch();
            InstantiateTracking();
        }

        public int GetProcessBarHeight()
        {
            return ProcessBar.Size.Height;
        }

        public void SetValue(int value)
        {
            ProcessBar.Value = value;
        }

        public int GetValue()
        {
            return ProcessBar.Value;
        }

        public void UpdateProcess(int value)
        {
            SetValue(value);
        }

        public void StartStopwatch()
        {
            Stopwatch.Start();
        }

        public void StopStopwatch()
        {
            Stopwatch.Stop();
        }

        public void UpdateTime()
        {
            TimeSpent.Text = $"{Stopwatch.Elapsed.Hours:00}:{Stopwatch.Elapsed.Minutes:00}:{Stopwatch.Elapsed.Seconds:00}";
        }

        /// <summary>
        /// Renders Process name label and progress bar
        /// </summary>
        private void InstantiateTracking()
        {
            ProcessLabel = new Label
            {
                Location = Location,
                Text = ProcessName,
                AutoSize = true
            };

            ProcessBar = new ProgressBar
            {
                Location = new Point(Location.X + ProcessLabel.Size.Width, Location.Y),
                Size = new Size(300, 20),
                Value = 100,
                AutoSize = true
            };

            TimeSpent = new Label
            {
                Location = new Point(ProcessBar.Location.X + ProcessBar.Size.Width + 10, Location.Y),
                Text = $"00:00:00",
                AutoSize = true
            };


            Controls.Add(ProcessBar);
            Controls.Add(ProcessLabel);
            Controls.Add(TimeSpent);
            StartStopwatch();
        }

    }
}
