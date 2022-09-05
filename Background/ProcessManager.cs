﻿using System;
using System.Windows.Forms;
using TimeTracker.Containers;
using TimeTracker.Entities;

namespace TimeTracker.Background
{
    internal class ProcessManager
    {
        ActiveProcess currentActiveProc;

        private static ProcessContainer processContainer;
        ProgressBarWorker worker;

        public ProcessManager()
        {
            processContainer = new ProcessContainer();
            worker = new ProgressBarWorker(processContainer);
            ProcessTicker();
        }

        public string GetActiveProcess()
        {
            if (currentActiveProc != null)
            {
                return currentActiveProc.ProcessName;
            }
            return null;
        }


        public bool ContainsProcess(string processName)
        {
            return processContainer.Contains(processName);
        }

        public void AddProcess(ActiveProcess process)
        {
            // if adding a process to the list, that means it is currently running, therefor it is the current active process.

            worker.AddProcess(process);
            
            //if (!worker.IsRunning())
            //{
            //    worker.Start();
            //}


            if (currentActiveProc != null)
            {
                currentActiveProc.Stop();

            }

            //PlaceInOrder(currentActiveProc);
            currentActiveProc = process;
            currentActiveProc.Start();

            processContainer.Add(process); // add at the end to not mess with previous processes
        }

        // temp method REMOVE WHEN NOT NEEDED
        public void AddProcessWithoutTracking(ActiveProcess process)
        {
            processContainer.Add(process);
        }

        public void ChangeActiveProcess(string processName)
        {
            if (ContainsProcess(processName))
            {
                currentActiveProc.Stop();
                //PlaceInOrder(currentActiveProc);
                currentActiveProc = processContainer.Get(processName);
                currentActiveProc.Start();
            }
        }

        public void UpdateActiveProcessBar()
        {
            currentActiveProc.UpdateTime();
            //_ = Task.Run(Test);
        }

        public void PlaceInOrder(Panel processNamePanel, Panel progressBarPanel, Panel timeSpentPanel)
        {
            var allProcesses = processContainer.GetAll();
            int alphaIndex;
            int betaIndex;
            ActiveProcess tmp;

            for (int i = 0; i < allProcesses.Count - 1; i++)
            {
                for (int j = 0; j < allProcesses.Count - 1; j++)
                {
                    var p1 = (ProgressBar)allProcesses[j].ProcessProgressPanel.Controls[0];
                    var p2 = (ProgressBar)allProcesses[j + 1].ProcessProgressPanel.Controls[0];

                    if (p1.Value > p2.Value)
                    {
                        alphaIndex = progressBarPanel.Controls.IndexOf(allProcesses[j + 1].ProcessProgressPanel);
                        betaIndex = progressBarPanel.Controls.IndexOf(allProcesses[j].ProcessProgressPanel);

                        progressBarPanel.Controls.SetChildIndex(allProcesses[j + 1].ProcessProgressPanel, betaIndex);
                        progressBarPanel.Controls.SetChildIndex(allProcesses[j].ProcessProgressPanel, alphaIndex);

                        processNamePanel.Controls.SetChildIndex(allProcesses[j + 1].ProcessPanel, betaIndex);
                        processNamePanel.Controls.SetChildIndex(allProcesses[j].ProcessPanel, alphaIndex);

                        timeSpentPanel.Controls.SetChildIndex(allProcesses[j + 1].ProcessTimePanel, betaIndex);
                        timeSpentPanel.Controls.SetChildIndex(allProcesses[j].ProcessTimePanel, alphaIndex);

                        tmp = allProcesses[j + 1];
                        allProcesses[j + 1] = allProcesses[j];
                        allProcesses[j] = tmp;
                    }
                }
            }
            // Set the new sorted process list to the process storage container
            processContainer = new ProcessContainer(allProcesses);
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
            foreach (var trackingProcess in processContainer.GetAll())
            {
                if (!(trackingProcess is null))
                {
                    trackingProcess.UpdateTime();
                }
            }
        }

        private void Test()
        {
            Console.WriteLine($"This is the test method running at {DateTime.Now}");
        }
    }
}
