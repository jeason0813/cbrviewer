#region Header
// *******************************************************************************************
// Authors     : Erik Molenaar
// *******************************************************************************************
#endregion // Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CBR_Viewer.Model
{
    public static class BackgroundLoadPages
    {
        //private BackgroundWorker worker;
        public delegate void WorkDelegateType(object sender, DoWorkEventArgs e);
        public delegate void WorkProgressDelegateType(object sender, ProgressChangedEventArgs e);
        public delegate void WorkCompleteDelegateType(object sender, RunWorkerCompletedEventArgs e);

        public static void DoBackground(WorkDelegateType work,
            WorkProgressDelegateType progress,
            WorkCompleteDelegateType complete,
            object argument)
        {
            BackgroundWorker worker = CreateBackground(work, progress, complete);
            worker.RunWorkerAsync(argument);
        }

        public static BackgroundWorker CreateBackground(WorkDelegateType work,
            WorkProgressDelegateType progress,
            WorkCompleteDelegateType complete)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;

            worker.DoWork += new DoWorkEventHandler(work);
            worker.ProgressChanged += new ProgressChangedEventHandler(progress);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(complete);
            return worker;
        }
    }
}
