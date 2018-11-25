using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteAnalyzer.Scanning
{
    public class ScanState
    {
        public bool SitemapLoaded { get; set; }
        public ScanProccessState ProccessState { get; set; }
        public int TotalSitemaps { get; set; }
        public int ScannedSitemaps { get; set; }
        public int TotalAddresses { get; set; }
        public int ScannedAddresses { get; set; }
        public string TextState { get; set; }
        public ScanState()
        {
            
        }

        public ScanState(bool sitemapLoaded, ScanProccessState proccessState, int totalSitemaps, int totalAddresses, int scannedAddresses, int scannedSitemaps)
        {
            SitemapLoaded = sitemapLoaded;
            ProccessState = proccessState;
            TotalSitemaps = totalSitemaps;
            TotalAddresses = totalAddresses;
            ScannedAddresses = scannedAddresses;
            ScannedSitemaps = scannedSitemaps;
        }
    }

    public enum ScanProccessState
    {
        Paused,
        Stopped,
        Scanning,
        Completed
    }
}