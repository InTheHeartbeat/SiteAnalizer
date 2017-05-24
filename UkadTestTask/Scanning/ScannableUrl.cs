using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UkadTestTask.Scanning
{
    public class ScannableUrl
    {
        public string Url { get; set; }
        public int LoadTimeMs { get; set; }
        public int Ping { get; set; }
        public bool Completed { get; set; }
    }
}