using SiteAnalyzer.Base;
using SiteAnalyzer.Scanning.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using SiteAnalyzer.Scanning.Extensions;

namespace SiteAnalyzer.Scanning
{
    public class SiteScanTask : ISiteScanTask
    {
        /// <summary>
        /// Current scanning web site
        /// </summary>
        public WebSite Site { get; set; }

        /// <summary>
        /// Result state scanning
        /// </summary>
        public ScanResult ResultState { get; private set; }

        /// <summary>
        /// Occurs when call method Stop(), but not at the actual stop.
        /// </summary>
        public event Action Stopped;
        /// <summary>
        /// Occurs when scanning is begun
        /// </summary>
        public event Action Started;
        /// <summary>
        /// Return true if site successful scanned
        /// </summary>       
        public bool Completed => Site != null
                                 && (Site.Sitemaps.All(t => t.Urls.All(u => u.Completed)) || !Site.Sitemaps.Any());

        private bool _cancellationPending;

        public SiteScanTask(WebSite site)
        {
            Site = site;
            ResultState = new ScanResult();
            _cancellationPending = false;
        }

        /// <summary>
        /// Start scanning
        /// </summary>
        public async Task Scan()
        {
            if (Site == null)
                throw new NullReferenceException($"Web site is null");
            if (string.IsNullOrWhiteSpace(Site.Url))
                throw new NullReferenceException($"Web site url is null or empty");

            await RunScanTask();
        }

        private async Task RunScanTask()
        {
            ResultState.ProccessState = ScanProccessState.Scanning;
            OnStarted();

            Stopwatch scanningTotalTimeSpent = Stopwatch.StartNew();

            ResultState.SiteResponseTime = new TimeSpan(0, 0, 0, 0, (int)await PingSite());

            if (!Site.SitemapLoaded)
                await LoadSitemap();

            await InterviewSite(scanningTotalTimeSpent);

            scanningTotalTimeSpent.Stop();
            ResultState.TotalTimeSpentScanning = scanningTotalTimeSpent.Elapsed;
            ResultState.ProccessState = ScanProccessState.Completed;
        }

        private async Task InterviewSite(Stopwatch scanningTotalTimeSpent)
        {
            foreach (Sitemap sitemap in Site.Sitemaps)
            {
                foreach (var notScannedUrl in sitemap.Urls.Where(u => !u.Completed))
                {
                    if (_cancellationPending)
                        return;

                    ResultState.TextState = "Scanning address " + notScannedUrl.Url + "(" +
                                      (ResultState.ScannedAddresses).ToString() + "/" +
                                      ResultState.TotalAddresses + ")";

                    try
                    {
                        SitemapUrl scannedUrl = await InterviewUrl(notScannedUrl);
                        notScannedUrl.LoadTime = scannedUrl.LoadTime;
                        notScannedUrl.Lenght = scannedUrl.Lenght;
                        notScannedUrl.Completed = true;
                    }
                    catch
                    {
                        notScannedUrl.LoadTime = TimeSpan.Zero;
                        notScannedUrl.Lenght = -1;
                        notScannedUrl.Completed = false;
                    }


                    ResultState.ScannedAddresses = ++ResultState.ScannedAddresses;
                    ResultState.PageLoadMinTime =
                        TimeSpan.FromMilliseconds(Site.Sitemaps.SelectMany(sm => sm.Urls).Min(url => url.LoadTime.TotalMilliseconds));
                    ResultState.PageLoadMaxTime =
                        TimeSpan.FromMilliseconds(Site.Sitemaps.SelectMany(sm => sm.Urls).Max(url => url.LoadTime.TotalMilliseconds));
                    ResultState.PageLoadAverageTime =
                        TimeSpan.FromMilliseconds(Site.Sitemaps.SelectMany(sm => sm.Urls).Average(url => url.LoadTime.TotalMilliseconds));
                    ResultState.ETA = scanningTotalTimeSpent.GetEta(ResultState.ScannedAddresses, ResultState.TotalAddresses);
                }

                ResultState.ScannedSitemaps = ++ResultState.ScannedSitemaps;
            }
        }

        private async Task LoadSitemap()
        {
            ResultState.TextState = "Loading sitemap";

            Stopwatch siteMapLoadTime = Stopwatch.StartNew();
            Site.Sitemaps = await (new SitemapProvider()).GetSitemapsFromUrl($"{Site.Url}/sitemap.xml");
            siteMapLoadTime.Stop();

            ResultState.SitemapLoadedTime = siteMapLoadTime.Elapsed;
            ResultState.SitemapLoaded = ResultState.SitemapLoaded = Site.SitemapLoaded;
            ResultState.TotalSitemaps = ResultState.TotalSitemaps = Site.Sitemaps.Count;
            ResultState.TotalAddresses = Site.Sitemaps.Sum(s => s.Urls.Count);
        }

        private async Task<SitemapUrl> InterviewUrl(SitemapUrl url)
        {
            using (HttpClient client = new HttpClient())
            {
                Stopwatch sw = Stopwatch.StartNew();

                string result = await client.GetStringAsync(url.Url);
                sw.Stop();
                url.LoadTime = sw.Elapsed;
                url.Lenght = result.Length;

                sw.Stop();

                return url;
            }
        }

        private async Task<long> PingSite()
            {
                try
                {
                    return (await new Ping().SendPingAsync(new Uri(Site.Url).Host)).RoundtripTime;
                }
                catch (Exception)
                {
                    ResultState.TextState = "Ping Error";
                    ResultState.ProccessState = ScanProccessState.Stopped;
                    return 0;
                }
            }

            public void Stop()
            {
                _cancellationPending = true;
                OnStopped();
            }


        protected virtual void OnStopped()
        {
            Stopped?.Invoke();
        }

        protected virtual void OnStarted()
        {
            Started?.Invoke();
        }
    }
}