using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UkadTestTask.Base;

namespace UkadTestTask.Models
{
    public class ResultDataModel
    {
        public Sitemap Sitemap;
        public int SitemapLoadedMs;
        public ResultDataModel(Sitemap sitemap, int milliseconds)
        {
            Sitemap = sitemap;
            SitemapLoadedMs = milliseconds;
        }
    }
}