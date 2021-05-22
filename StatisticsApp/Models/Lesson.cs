using RDotNet;
using System.Collections.Generic;

namespace StatisticsApp.Models
{
    public class Lesson
    {
        public DataFrame Dataset { get; set; }
        public string UserDatasetLocation { get; set; }
        public List<string> DefaultDatasetLocations { get; set; }
        public string RCodeLocation { get; set; }
    }
}
