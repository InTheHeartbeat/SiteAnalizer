using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Xml.Serialization;
using WebGrease.Css.Extensions;

namespace UkadTestTask.Scanning
{
    public class Scanner : IRegisteredObject
    {
        private readonly string _storagePath;
        private readonly object _locker = new object();

        private bool Cancelled { get; set; }

            public List<SiteScanTask> ScanTasks { get; private set; }

        public Scanner(string storagePath)
        {
            _storagePath = storagePath;
            Cancelled = false; 
            LoadTasksFromStorage();           
        }

        private void RunIncomplete()
        {
            ScanTasks?.Where(t=>!t.Completed).ForEach(t=>t.Run());
        }
        

        private void LoadTasksFromStorage()
        {
            lock (_locker)
            {
                if (File.Exists(_storagePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<SiteScanTask>));
                    using (FileStream fs = new FileStream(_storagePath, FileMode.OpenOrCreate))
                    {
                        ScanTasks = serializer.Deserialize(fs) as List<SiteScanTask>;
                    }
                }
                else
                {
                    ScanTasks = new List<SiteScanTask>();
                    XmlSerializer serializer = new XmlSerializer(typeof(List<SiteScanTask>));
                    using (FileStream fs = new FileStream(_storagePath, FileMode.OpenOrCreate))
                    {
                        serializer.Serialize(fs, ScanTasks);
                    }
                }
            }
        }

        private void StopAndSaveOperations()
        {
            if (!Cancelled)
            {
                Cancelled = true;
                lock (_locker)
                {
                    ScanTasks.Where(t=>t.IsRunning).ForEach(t=>t.Stop());
                    while (ScanTasks.Any(t=>t.IsRunning))
                    {
                        Thread.Sleep(50);
                    }

                    XmlSerializer serializer = new XmlSerializer(typeof(List<SiteScanTask>));
                    using (FileStream fs = new FileStream(_storagePath, FileMode.OpenOrCreate))
                    {
                        serializer.Serialize(fs, ScanTasks);
                    }
                }
            }
        }


        public void Stop(bool immediate)
        {
            
        }
    }
}