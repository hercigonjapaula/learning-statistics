using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace StatisticsApp.Models
{
    public class IndependenceViewModel
    {
        public string Variable1 { get; set; }
        public string Variable2 { get; set; }
        public string AlternativeHypothesis { get; set; }
        public List<SelectListItem> Variables1 { get; set; }
        public List<SelectListItem> Variables2 { get; set; }
        public List<SelectListItem> AlternativeHypotheses { get; set; }
    }
}
