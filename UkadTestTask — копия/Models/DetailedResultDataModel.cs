using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SiteAnalyzer.Base;

namespace SiteAnalyzer.Models
{
    public class DetailedResultDataModel
    {
        public WebSite Site { get; set; }
        public DetailedResultDataModel(WebSite site)
        {
            Site = site;
        }
    }
}