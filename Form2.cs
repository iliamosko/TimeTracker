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

        private static ProcessUpdater ProcessUpdater;

        public Form2()
        {
            start = DateTime.UtcNow;
            
            InitializeComponent();
            panel1.AutoScroll = true;
            ProcessUpdater = new ProcessUpdater();
            TimeElapsed();
            ProcessTracker.SetupTracking(panel1, ProcessUpdater);
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
            ProcessTracker.TrackProcess();
        }
    }
}
