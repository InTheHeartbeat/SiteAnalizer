using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using UkadTestTask.Base;

namespace UkadTestTask.Scanning
{
    [Serializable]
    public class SiteScanTask
    {
        [XmlIgnore]
        private BackgroundWorker _scanningWorker;


        [XmlIgnore]
        public bool SitemapLoaded => Sitemap != null && Sitemap.RootUris.Count > 0 && Sitemap.Uris.Count > 0;
        [XmlIgnore]
        public bool SiteScanned => Sitemap != null && Sitemap.Uris != null && Sitemap.Uris.

        [XmlIgnore]
        public bool Completed => SiteScanned && SitemapLoaded;

        public SiteScanTask()
        {
                
        }
    }
}