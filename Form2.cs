using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeTracker
{
    public partial class Form2 : Form
    {
        DateTime start;

        public List<Process> Processes { get; private set; }

        private Point InitialPoint = new Point(200, 80);
        
        public Form2()
        {
            InitializeComponent();
            panel1.AutoScroll = true;
            start = DateTime.UtcNow;
            TimeElapsed();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Processes is null)
            {
                Processes = new List<Process>
                {
                    new Process("Process #1", Controls, InitialPoint)
                };
            }
            else
            {
                var proc = Processes.ElementAt(Processes.Count() - 1);

                InitialPoint = new Point(InitialPoint.X, InitialPoint.Y + proc.GetProcessBarHeight() + 5);
                Processes.Add(new Process($"Process #{Processes.Count() + 1}", Controls, InitialPoint));
            }

            
        }

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
        }
    }
}
