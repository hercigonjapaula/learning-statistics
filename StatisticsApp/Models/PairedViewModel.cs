using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace StatisticsApp.Models
{
    public class PairedViewModel
    {
        public string AlternativeHypothesis { get; set; }
        public string ConfidenceInterval { get; set; }   
        public List<SelectListItem> AlternativeHypotheses { get; set; }
    }
}
