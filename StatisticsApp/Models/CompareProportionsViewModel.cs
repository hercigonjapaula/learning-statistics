using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatisticsApp.Models
{
    public class CompareProportionsViewModel
    {
        public string Variable1 { get; set; }
        public string Variable2 { get; set; }
        public string Level1 { get; set; }
        public string Level21 { get; set; }
        public string Level22 { get; set; }        
        public string AlternativeHypothesis { get; set; }        
        public List<SelectListItem> Variables { get; set; }        
        public List<SelectListItem> Levels1 { get; set; }
        public List<SelectListItem> Levels2 { get; set; }       
        public List<SelectListItem> AlternativeHypotheses { get; set; }
    }
}
