using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace TimeTracker
{
    public class Process
    {
        /// <summary>
        /// Gets the process name
        /// </summary>
        string ProcessName { get; }

        /// <summary>
        /// Gets the processes controls
        /// </summary>
        ControlCollection Controls { get; }

        /// <summary>
        /// Gets the location of the label
        /// </summary>
        Point Location { get; }

        Label ProcessLabel;
        ProgressBar ProcessBar;

        /// <summary>
        /// Creates tracking information with the given process name, panel controls and the location.
        /// </summary>
        /// <param name="processName">The process name</param>
        /// <param name="controls">The controls for the process</param>
        /// <param name="location">The location of the components</param>
        public Process(string processName, ControlCollection controls, Point location)
        {
            ProcessName = processName;
            Controls = controls;
            Location = location;
            InstantiateTracking();
        }

        public int GetProcessBarHeight()
        {
            return ProcessBar.Size.Height;
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
                Value = 10,
                AutoSize = true
            };

            Controls.Add(ProcessBar);
            Controls.Add(ProcessLabel);
        }

    }
}
