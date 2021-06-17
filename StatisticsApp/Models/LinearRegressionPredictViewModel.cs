using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatisticsApp.Models
{
    public class LinearRegressionPredictViewModel
    {
        public string X { get; set; }
        public string Y { get; set; }
        public List<SelectListItem> Variables { get; set; }
    }
}
