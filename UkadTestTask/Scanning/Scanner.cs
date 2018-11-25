using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Xml.Serialization;
using SiteAnalyzer.Base;
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

        public void ScanSite(string url, bool append)
        {
            if(Cancelled) throw new InvalidOperationException("Service stopped.");

            //lock (_locker)
            //{              
                SiteScanTask task = new SiteScanTask(new WebSite(url));
                ScanTasks.Add(task);
                task.Scan();
            //}
        }


        public void StopScanningSite(string url)
        {
            if (Cancelled) throw new InvalidOperationException("Service stopped.");

            lock (_locker)
            {
                ScanTasks.FirstOrDefault(t=>t.Site.Url == url && !t.Completed)?.Stop();
            }
        }

        private void RunIncomplete()
        {
            lock (_locker)
            {
                ScanTasks?.Where(t => !t.Completed).ForEach(t => t.Scan());
            }
        }                

        private void LoadTasksFromStorage()
        {}        

        public void Stop(bool immediate)
        {
            StopAndSaveOperations();
        }

        private void StopAndSaveOperations()
        {}
    }
}