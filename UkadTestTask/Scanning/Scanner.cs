using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace UkadTestTask.Scanning
{
    public class Scanner : IRegisteredObject
    {               
        private bool Cancelled { get; set; }

        public Scanner()
        {
            _storagePath = storagePath;
            Cancelled = false;
        }

        public void Stop(bool immediate)
        {
            
        }
    }
}