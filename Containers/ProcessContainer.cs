using System;
using System.Collections.Generic;
using TimeTracker.Entities;
using TimeTracker.Interfaces;

namespace TimeTracker.Containers
{
    internal class ProcessContainer : IStorage<ActiveProcess>
    {
        List<ActiveProcess> Storage;

        public ProcessContainer()
        {
            Storage = new List<ActiveProcess>();
        }

        public ProcessContainer(List<ActiveProcess> processList)
        {
            Storage = processList;
        }

        /// <summary>
        /// Adds a process to the storage
        /// </summary>
        /// <param name="process">The process to add</param>
        /// <returns>Returns true if the process has been added successfully</returns>
        public bool Add(ActiveProcess process)
        {
            if (!Storage.Contains(process))
            {
                Storage.Add(process);
                return true;
            }
            return false;
        }

        public ActiveProcess Get(ActiveProcess item)
        {
            throw new NotImplementedException();
        }

        public ActiveProcess Get(string processName)
        {
            foreach (var process in Storage)
            {
                if (processName == process.ProcessName)
                {
                    return process;
                }
            }
            return null;
        }

        /// <summary>
        /// Checks if a process already exists
        /// </summary>
        /// <param name="processName">The name of the process</param>
        /// <returns>True if process is found, False otherwise</returns>
        public bool Contains(string processName)
        {
            foreach (var proc in Storage)
            {
                if (proc.ProcessName == processName)
                    return true;
            }
   
            return false;
        }

        /// <summary>
        /// Removes a <see cref="ActiveProcess"/> from the container
        /// </summary>
        /// <param name="process">The <see cref="ActiveProcess"/> to remove</param>
        /// <returns> </returns>
        public bool Remove(ActiveProcess process)
        {
            if (Storage.Contains(process))
            {
                Storage.Remove(process);
                return true;
            }
            return false;
        }

        public List<ActiveProcess> GetAll()
        {
            if (Storage != null)
            {
                return new List<ActiveProcess>(Storage);
            }
            return null;
        }
    }
}
