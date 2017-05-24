using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UkadTestTask.Scanning
{
    [Serializable]
    public class ScannableUrl
    {
        public ScannableUrl(string url)
        {
            Url = url;
        }

        public string Url { get; set; }
        public long LoadTimeMs { get; set; }        
        public bool Completed { get; set; }
        public long Lenght { get; set; }
    }
}