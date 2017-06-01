using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using UkadTestTask.Scanning;

namespace UkadTestTask.Controllers
{
    public class StateProviderController : ApiController
    {
        public ScanState Get([FromUri]string url)
        {            
            SiteScanTask current = ScannerProvider.Scanner.ScanTasks.First(t => t.Site.Url == url);
            return current.State;
        }

        [HttpGet]
        public ScanState GetState(string url)
        {
           SiteScanTask current = ScannerProvider.Scanner.ScanTasks.FirstOrDefault(t => t.Site.Url == Encoding.UTF8.GetString(Convert.FromBase64String(url)));
            return current.State;
        }
    }
}
