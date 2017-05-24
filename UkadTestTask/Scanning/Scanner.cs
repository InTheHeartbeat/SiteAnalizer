using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace UkadTestTask.Scanning
{
    public class Scanner : IRegisteredObject
    {
        private readonly string _storagePath;

        private bool Cancelled { get; set; }

        public Scanner(string storagePath)
        {
            _storagePath = storagePath;
            Cancelled = false;
        }

        private void RunIncomplete()
        { }

        private void LoadTasksFromStorage()
        { }

        private void StopAndSaveOperations()
        { }


        public void Stop(bool immediate)
        {
            
        }
    }
}