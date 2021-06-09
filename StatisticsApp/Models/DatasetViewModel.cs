using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace StatisticsApp.Models
{
    public class DatasetViewModel
    {
        public string Dataset { get; set; }
        public string Variable { get; set; }
        public string Plot { get; set; }
        public string ChangedRCode { get; set; }
        public List<SelectListItem> Datasets { get; set; }
        public List<SelectListItem> Variables { get; set; }
        public List<SelectListItem> Plots { get; set; }
    }
}
