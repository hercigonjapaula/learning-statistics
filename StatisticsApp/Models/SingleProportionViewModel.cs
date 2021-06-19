using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace StatisticsApp.Models
{
    public class SingleProportionViewModel
    {
        public string Variable { get; set; }
        public string Level { get; set; }
        public string NullHypothesis { get; set; }
        public string AlternativeHypothesis { get; set; }
        public string ConfidenceInterval { get; set; }        
        public List<SelectListItem> Variables { get; set; }
        public List<SelectListItem> Levels { get; set; }
        public List<SelectListItem> AlternativeHypotheses { get; set; }
    }
}
