using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace StatisticsApp.Models
{
    public class SingleViewModel
    {
        public string Dataset { get; set; }
        public string Variable { get; set; }
        public string Test { get; set; }
        public string NullHypothesis { get; set; }
        public string AlternativeHypothesis { get; set; }
        public string ConfidenceInterval { get; set; }      
        public List<SelectListItem> Tests { get; set; }
        public List<SelectListItem> Datasets { get; set; }
        public List<SelectListItem> Variables { get; set; }
        public List<SelectListItem> AlternativeHypotheses { get; set; }    
    }
}
