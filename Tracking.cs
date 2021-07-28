using System;
using System.Windows.Forms;

namespace TimeTracker
{
    public partial class Tracking : Form
    {
        readonly DateTime start;

        private static ProcessUpdater ProcessUpdater;

        public static TimeSpan TimeDifference;

        public Tracking()
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
            TimeDifference = currentTime - start;
            var hours = TimeDifference.Hours;
            var minutes = TimeDifference.Minutes;
            var seconds = TimeDifference.Seconds;
            label2.Text = $"{hours:00}:{minutes:00}:{seconds:00}";

            ProcessTracker.TrackProcess();
        }
    }
}
