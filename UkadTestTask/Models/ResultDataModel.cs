using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UkadTestTask.Base;
using UkadTestTask.Scanning;

namespace UkadTestTask.Models
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