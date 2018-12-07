using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteAnalyzer.Scanning
{
    [Serializable]
    public class SitemapUrl
    {        
        public string Url { get; set; }
        public TimeSpan LoadTime { get; set; }        
        public bool Completed { get; set; }
        public long Lenght { get; set; }

        public SitemapUrl()
        { }

        public SitemapUrl(string url)
        {
            Url = url;
        }
    }
}