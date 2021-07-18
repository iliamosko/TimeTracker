using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker
{
    public class ProcessUpdater
    {
        private List<TrackingProcess> Processes { get; set; }

        public ProcessUpdater()
        {
            Processes = new List<TrackingProcess>();
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
            if(Processes.Exists(process => process.ProcessName.Equals(proc)))
            {
                return true;
            }

            return false;
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

    }
}
