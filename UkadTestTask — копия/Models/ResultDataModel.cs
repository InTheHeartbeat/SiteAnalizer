using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SiteAnalyzer.Base;
using SiteAnalyzer.Scanning;

namespace SiteAnalyzer.Models
{
    public class ResultDataModel
    {
        public WebSite Site;        
        public ScanResult Result;

        public ResultDataModel(WebSite site, ScanResult result)
        {
            Site = site;
            Result = result;
        }
    }
}