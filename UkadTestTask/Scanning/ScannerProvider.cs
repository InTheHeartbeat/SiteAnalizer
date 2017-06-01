using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UkadTestTask.Scanning
{
    public class ScannerProvider
    {
        public static Scanner Scanner { get; private set; }

        public static void Initialize()
        {
            if (Scanner == null)
            {
                Scanner = new Scanner();
            }
        }
    }
}