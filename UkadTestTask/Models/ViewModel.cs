using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteAnalyzer.Models
{
    public class ViewModel
    {
        public bool NavBarDefault { get; set; }

        public ViewModel()
        {
            NavBarDefault = true;
        }
    }
}