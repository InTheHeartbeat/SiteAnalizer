using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteAnalyzer.Scanning
{
    public class ScanResult : ScanState
    {        
        public TimeSpan TotalTimeSpentScanning { get; set; }
        public TimeSpan SiteResponseTime { get; set; }
        public TimeSpan PageLoadMinTime { get; set; }
        public TimeSpan PageLoadAverageTime { get; set; }
        public TimeSpan PageLoadMaxTime { get; set; }
        public TimeSpan SitemapLoadedTime { get; set; }
        public TimeSpan ETA { get; set; }
        public string ETAText => $"Remaining {ETA:hh\\:mm\\:ss}";
        public ScanResult()
        {
            
        }

        public ScanResult(TimeSpan totalTimeSpentScanning, TimeSpan siteResponseTime, TimeSpan minPageLoadTime, TimeSpan maxPageLoadTime, bool sitemapLoaded, ScanProccessState proccessState, int totalSitemaps, int totalAddresses, int scannedAddresses, TimeSpan middlePageLoadedTime, TimeSpan sitemapLoadedTime, int scannedSitemaps):base(sitemapLoaded,proccessState,totalSitemaps,totalAddresses,scannedAddresses,scannedSitemaps)
        {
            TotalTimeSpentScanning = totalTimeSpentScanning;
            SiteResponseTime = siteResponseTime;
            PageLoadMinTime = minPageLoadTime;
            PageLoadMaxTime = maxPageLoadTime;
            PageLoadAverageTime = middlePageLoadedTime;
            SitemapLoadedTime = sitemapLoadedTime;
        }
    }
}