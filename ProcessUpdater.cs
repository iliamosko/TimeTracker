using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TimeTracker
{
    public class ProcessUpdater
    {
        private List<TrackingProcess> Processes { get; set; }

        private TrackingProcess CurrentActiveProcess;

        private TrackingProcess PreviousActiveProcess;

        public ProcessUpdater()
        {
            Processes = new List<TrackingProcess>();
            ProcessTicker();
        }

        public int TotalTrackedProcess()
        {
            return Processes.Count();
        }

        /// <summary>
        /// Add a process to track
        /// </summary>
        /// <param name="proc">The process to track</param>
        public void AddProcess(TrackingProcess proc)
        {
            if (!(proc is null))
            {
                Processes.Add(proc);
                SetActiveProcess(proc);
            }
            else
            {
                throw new NullReferenceException("Unable to track process of type: null");
            }
        }

        /// <summary>
        /// Checks the list of current processes to see if the requested one exists
        /// </summary>
        /// <param name="proc">The String representation of the process name</param>
        /// <returns>returns <see cref="True"/> if process exists, <see cref="False"/> otherwise</returns>
        public bool HasProcess(string proc)
        {
            if(proc is null)
            {
                return false;
            }
            
            if(Processes.Exists(process => process.ProcessName.Equals(proc)))
            {
                return true;
            }

            return false;
        }

        public TrackingProcess GetProcess(string proc)
        {
            var foundProcess = Processes.FirstOrDefault(process => process.ProcessName.Equals(proc));

            if(foundProcess is null)
            {
                throw new ArgumentNullException("There is no such process");
            }
            return foundProcess;
        }

        /// <summary>
        /// Returns the last index in the list of processes
        /// </summary>
        /// <returns>Returns the last added process</returns>
        public TrackingProcess LastAddedProcess()
        {
            if (Processes.Count == 0)
            {
                throw new Exception("No processes being tracked");
            }

            return Processes.LastOrDefault();
        }

        /// <summary>
        /// Sets the current active process. Starts and stops the <see cref="TrackingProcess.Stopwatch"/> to the respective process
        /// whenever the focus is switched.
        /// </summary>
        /// <param name="proc">The current active <see cref="TrackingProcess"/></param>
        public void SetActiveProcess(TrackingProcess proc)
        {
            if(CurrentActiveProcess is null)
            {
                CurrentActiveProcess = proc;
                CurrentActiveProcess.StartStopwatch();
            }
            else if (CurrentActiveProcess.ProcessName != proc.ProcessName)
            {
                CurrentActiveProcess.StopStopwatch();
                PreviousActiveProcess = CurrentActiveProcess;
                CurrentActiveProcess = proc;
                CurrentActiveProcess.StartStopwatch();
            }
        }

        public TrackingProcess GetCurrentActiveProcess()
        {
            return CurrentActiveProcess ?? throw new Exception("No active process");
        }

        /// <summary>
        /// Checks the list for any active processes
        /// </summary>
        /// <returns>returns <see cref="False"/> if there are no currently tracked processes, <see cref="True"/> otherwise</returns>
        public bool ContainsProcesses()
        {
            if(Processes is null || Processes.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ProcessTicker()
        {
            Timer timer = new Timer
            {
                Interval = (1 * 1000) //1 second
            };
            timer.Tick += new EventHandler(UpdateProgressHandler);
            timer.Start();
        }

        private void UpdateProgressHandler(object sender, EventArgs e)
        {
            foreach(var trackingProcess in Processes)
            {
                if(!(trackingProcess is null))
                {
                    trackingProcess.UpdateTime();
                }
            }
            SortAndSwap();
            //SwapProcess();
        }

        private void SortAndSwap()
        {
            var sortedProcesses = Processes.OrderByDescending(proc => proc.GetProgressBarLength()).ToList();
            Console.WriteLine(Processes.First().ProcessName);
            Console.WriteLine(sortedProcesses.First().ProcessName);

            if (!sortedProcesses.SequenceEqual(Processes))
            {
                var x = sortedProcesses.First();
                var t = Processes.First();
                t.Swap(x);
            }
            //Processes = sortedProcesses;
            Console.WriteLine(Processes.First().ProcessName);
        }

        private void SwapProcess()
        {
            if(!(PreviousActiveProcess is null))
            {
                if (CurrentActiveProcess.GetProgressBarLength() > PreviousActiveProcess.GetProgressBarLength())
                {
                    // Perform swapping operation
                    string tempName = PreviousActiveProcess.ProcessName;
                    int tempLength = PreviousActiveProcess.GetProgressBarLength();

                    PreviousActiveProcess.ProcessName = CurrentActiveProcess.ProcessName;
                    PreviousActiveProcess.SetProgressBarValue(CurrentActiveProcess.GetProgressBarLength());

                    CurrentActiveProcess.ProcessName = tempName;
                    CurrentActiveProcess.SetProgressBarValue(tempLength);

                    CurrentActiveProcess = PreviousActiveProcess;
                    //PreviousActiveProcess = CurrentActiveProcess;
                }
            }
        }
    }
}
