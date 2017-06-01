using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UkadTestTask.Scanning
{
    public class ScanResult : ScanState
    {
        public TimeSpan TotalTimeSpentScanning { get; set; }
        public TimeSpan SiteResponseTime { get; set; }
        public TimeSpan PageLoadedMinTime { get; set; }
        public TimeSpan PageLoadedMiddleTime { get; set; }
        public TimeSpan PageLoadedMaxTime { get; set; }
        public TimeSpan SitemapLoadedTime { get; set; }
        public ScanResult()
        {
            
        }

        public ScanResult(TimeSpan totalTimeSpentScanning, DateTime siteResponseTime, TimeSpan minPageLoadedTime, TimeSpan maxPageLoadedTime, bool sitemapLoaded, ScanProccessState proccessState, int totalSitemaps, int totalAddresses, int scannedAddresses, TimeSpan middlePageLoadedTime, TimeSpan sitemapLoadedTime, int scannedSitemaps):base(sitemapLoaded,proccessState,totalSitemaps,totalAddresses,scannedAddresses,scannedSitemaps)
        {
            TotalTimeSpentScanning = totalTimeSpentScanning;
            SiteResponseTime = siteResponseTime;
            PageLoadedMinTime = minPageLoadedTime;
            PageLoadedMaxTime = maxPageLoadedTime;
            PageLoadedMiddleTime = middlePageLoadedTime;
            SitemapLoadedTime = sitemapLoadedTime;
        }
    }
}