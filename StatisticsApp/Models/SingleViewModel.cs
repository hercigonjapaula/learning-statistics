using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace StatisticsApp.Models
{
    public class SingleViewModel
    {        
        public string Variable { get; set; }
        public string Test { get; set; }
        public double NullHypothesis { get; set; }
        public string AlternativeHypothesis { get; set; }
        public double ConfidenceInterval { get; set; }      
        public List<SelectListItem> Tests { get; set; }
        public List<SelectListItem> Variables { get; set; }
        public List<SelectListItem> AlternativeHypotheses { get; set; }    
    }
}
