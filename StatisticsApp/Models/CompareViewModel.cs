using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace StatisticsApp.Models
{
    public class CompareViewModel
    {
        public string Dataset1 { get; set; }
        public string Dataset2 { get; set; }
        public string Test { get; set; }      
        public string AlternativeHypothesis { get; set; }
        public string ConfidenceInterval { get; set; }
        public List<SelectListItem> Tests { get; set; }       
        public List<SelectListItem> AlternativeHypotheses { get; set; }
    }
}
