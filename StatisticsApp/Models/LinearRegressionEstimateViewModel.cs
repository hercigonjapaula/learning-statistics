using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace StatisticsApp.Models
{
    public class LinearRegressionEstimateViewModel
    {
        public string X { get; set; }
        public string Y { get; set; }
        public List<SelectListItem> Variables { get; set; }
    }
}
