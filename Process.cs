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
        string ProcessName { get; }

        ControlCollection Controls { get; }

        Point Location { get; }

        Label ProcessLabel;

        ProgressBar ProcessBar;

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
