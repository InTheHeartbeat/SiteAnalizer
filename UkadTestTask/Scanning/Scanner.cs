﻿using SiteAnalyzer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using WebGrease.Css.Extensions;

namespace SiteAnalyzer.Scanning
{
    public class Scanner : IRegisteredObject
    {
        private readonly object _locker = new object();

        private bool Cancelled { get; set; }

        public List<SiteScanTask> ScanTasks { get; private set; }

        public Scanner()
        {
            Cancelled = false;
            LoadTasksFromStorage();
            RunIncomplete();
            ScanTasks = new List<SiteScanTask>();
        }

        public async Task ScanSite(string url, bool append)
        {
            if (Cancelled) throw new InvalidOperationException("Service stopped.");
            
            SiteScanTask task = new SiteScanTask(new WebSite(url));
            ScanTasks.Add(task);
            await task.Scan();
        }


        public void StopScanningSite(string url)
        {
            if (Cancelled) throw new InvalidOperationException("Service stopped.");

            lock (_locker)
            {
                ScanTasks.FirstOrDefault(t => t.Site.Url == url && !t.Completed)?.Stop();
            }
        }

        private void RunIncomplete()
        {
            lock (_locker)
            {
                ScanTasks?.Where(t => !t.Completed).ForEach( async t => await t.Scan());
            } 
        }

        private void LoadTasksFromStorage()
        { }

        public void Stop(bool immediate)
        {
            StopAndSaveOperations();
        }

        private void StopAndSaveOperations()
        { }
    }
}