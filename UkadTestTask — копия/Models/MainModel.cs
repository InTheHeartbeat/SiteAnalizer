using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SiteAnalyzer.Scanning;

namespace SiteAnalyzer.Models
{
    public class MainModel
    {
        public string Url { get; set; }
        public bool Completed { get; set; }
        public bool SitemapContain { get; set; }
        public List<SiteScanTask> Tasks { get; set; }
        public MainModel()
        {            
        }        
    }
}