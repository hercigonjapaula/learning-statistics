using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StatisticsApp.Models;

namespace StatisticsApp.Controllers
{
    public class HomeController : Controller
    {
        public static CSharpR CSharpR = new CSharpR("C:/Program Files/R/R-4.0.5/bin/x64/Rscript.exe");
        public static string[] Lines = System.IO.File.ReadAllLines("C:/Users/Paula/Desktop/FER-10.semestar/" +
                    "StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/iris.csv");
        public static List<SelectListItem> Datasets = new List<SelectListItem>()
        {
                    new SelectListItem() { Text="iris", Value="iris" },
                    new SelectListItem() { Text="mtcars", Value="mtcars" },
                    new SelectListItem() { Text="PlantGrowth", Value="PlantGrowth" },
                    new SelectListItem() { Text="ToothGrowth", Value="ToothGrowth" },
        };
        public static string Dataset;
        public static List<SelectListItem> Variables;
        public static List<SelectListItem> Plots = new List<SelectListItem>()
        {
                    new SelectListItem() { Text="histogram", Value="histogram" },
                    new SelectListItem() { Text="box plot", Value="boxplot" },
                    new SelectListItem() { Text="scatter plot", Value="scatterplot" }
        };
        public static string[] RCode = System.IO.File.ReadAllLines("C:/Users/Paula/Desktop/FER-10.semestar/lesson.r");


        public IActionResult Index()
        {
            Variables = new List<SelectListItem>();
            int counter = 1;
            foreach (string variable in Lines[0].Split(",").Select(x => x = x.Replace("\"", "")))
            {
                Variables.Add(new SelectListItem() { Text = variable, Value = counter.ToString() });
                counter++;
            }
            DatasetViewModel datasetViewModel = new DatasetViewModel
            {
                Dataset = "iris",
                Variable = Variables[0].Value,
                Plot = Plots[0].Value,
                Datasets = Datasets,
                Variables = Variables,
                Plots = Plots
            };
            Dataset = "iris";
            string outputText = CSharpR.ExecuteRScript("C:/Users/Paula/Desktop/FER-10.semestar/lesson.r",
                new string[] { datasetViewModel.Dataset, datasetViewModel.Variable, datasetViewModel.Plot },
                out string standardError);
            outputText = CSharpR.ExecuteRScript("C:/Users/Paula/Desktop/FER-10.semestar/summary.r",
                new string[] { datasetViewModel.Dataset, datasetViewModel.Variable, datasetViewModel.Plot },
                out string stdError);
            string[] lines = System.IO.File.ReadAllLines(
                "C:/Users/Paula/Desktop/FER-10.semestar/StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/summary.txt");
            string[] summary = lines[1].Trim().Split("   ");
            ViewBag.Min = summary[0];
            ViewBag.FirstQ = summary[1];
            ViewBag.Median = summary[2];
            ViewBag.Mean = summary[3];
            ViewBag.ThirdQ = summary[4];
            ViewBag.Max = summary[5];
            ViewBag.Dataset = Lines;          
            ViewBag.RCode = RCode;
            return View(datasetViewModel);
        }
  
        [HttpPost]
        public IActionResult ChangeDataset(DatasetViewModel datasetViewModel)
        {
            Dataset = datasetViewModel.Dataset;
            Lines = System.IO.File.ReadAllLines("C:/Users/Paula/Desktop/FER-10.semestar/" +
                    "StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/" 
                    + datasetViewModel.Dataset + ".csv");
            Variables = new List<SelectListItem>();
            int counter = 1;
            foreach (string variable in Lines[0].Split(",").Select(x => x = x.Replace("\"", "")))
            {
                Variables.Add(new SelectListItem() { Text = variable, Value = counter.ToString() });
                counter++;
            }
            datasetViewModel.Datasets = Datasets;
            datasetViewModel.Variable = Variables[0].Value;
            datasetViewModel.Variables = Variables;
            datasetViewModel.Plots = Plots;
            datasetViewModel.Plot = Plots[0].Value;
            string outputText = CSharpR.ExecuteRScript("C:/Users/Paula/Desktop/FER-10.semestar/lesson.r",
                new string[] { datasetViewModel.Dataset, datasetViewModel.Variable, datasetViewModel.Plot },
                out string standardError);
            outputText = CSharpR.ExecuteRScript("C:/Users/Paula/Desktop/FER-10.semestar/summary.r",
                new string[] { datasetViewModel.Dataset, datasetViewModel.Variable, datasetViewModel.Plot },
                out string stdError);
            string[] lines = System.IO.File.ReadAllLines(
                "C:/Users/Paula/Desktop/FER-10.semestar/StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/summary.txt");
            string[] summary = lines[1].Trim().Split("   ");
            ViewBag.Min = summary[0];
            ViewBag.FirstQ = summary[1];
            ViewBag.Median = summary[2];
            ViewBag.Mean = summary[3];
            ViewBag.ThirdQ = summary[4];
            ViewBag.Max = summary[5];
            ViewBag.Dataset = Lines;
            ViewBag.RCode = RCode;
            return View("Index", datasetViewModel);
        }

        [HttpPost]
        public IActionResult ChangeVariableAndPlot(DatasetViewModel datasetViewModel)
        {
            datasetViewModel.Datasets = Datasets;
            datasetViewModel.Dataset = Dataset;
            datasetViewModel.Variables = Variables;
            datasetViewModel.Plots = Plots;            
            string outputText = CSharpR.ExecuteRScript("C:/Users/Paula/Desktop/FER-10.semestar/lesson.r",
                new string[] { datasetViewModel.Dataset, datasetViewModel.Variable, datasetViewModel.Plot }, 
                out string standardError);
            outputText = CSharpR.ExecuteRScript("C:/Users/Paula/Desktop/FER-10.semestar/summary.r",
                new string[] { datasetViewModel.Dataset, datasetViewModel.Variable, datasetViewModel.Plot },
                out string stdError);
            string[] lines = System.IO.File.ReadAllLines(
                "C:/Users/Paula/Desktop/FER-10.semestar/StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/summary.txt");
            string[] summary = lines[1].Trim().Split("   ");
            ViewBag.Min = summary[0];
            ViewBag.FirstQ = summary[1];
            ViewBag.Median = summary[2];
            ViewBag.Mean = summary[3];
            ViewBag.ThirdQ = summary[4];
            ViewBag.Max = summary[5];
            ViewBag.Dataset = Lines;
            ViewBag.RCode = RCode;
            return View("Index", datasetViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Content("File not selected");
            }
            var path = Path.Combine("C:/Users/Paula/Desktop/FER-10.semestar/" +
                    "StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/" + 
                    file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            Lines = System.IO.File.ReadAllLines("C:/Users/Paula/Desktop/FER-10.semestar/" +
                    "StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/"
                    + file.FileName);
            Variables = new List<SelectListItem>();
            int counter = 1;
            foreach (string variable in Lines[0].Split(",").Select(x => x = x.Replace("\"", "")))
            {
                Variables.Add(new SelectListItem() { Text = variable, Value = counter.ToString() });
                counter++;
            }
            DatasetViewModel datasetViewModel = new DatasetViewModel
            {
                Datasets = Datasets,
                Variable = Variables[0].Value,
                Variables = Variables,
                Plot = Plots[0].Value,
                Plots = Plots
            };
            Dataset = "C:/Users/Paula/Desktop/FER-10.semestar/" +
                    "StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/" +
                    file.FileName;            
            string outputText = CSharpR.ExecuteRScript("C:/Users/Paula/Desktop/FER-10.semestar/lesson.r",
                new string[] { Dataset, datasetViewModel.Variable, datasetViewModel.Plot },
                out string standardError);
            outputText = CSharpR.ExecuteRScript("C:/Users/Paula/Desktop/FER-10.semestar/summary.r",
                new string[] { datasetViewModel.Dataset, datasetViewModel.Variable, datasetViewModel.Plot },
                out string stdError);
            string[] lines = System.IO.File.ReadAllLines(
                "C:/Users/Paula/Desktop/FER-10.semestar/StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/summary.txt");
            string[] summary = lines[1].Trim().Split("   ");
            ViewBag.Min = summary[0];
            ViewBag.FirstQ = summary[1];
            ViewBag.Median = summary[2];
            ViewBag.Mean = summary[3];
            ViewBag.ThirdQ = summary[4];
            ViewBag.Max = summary[5];
            ViewBag.Dataset = Lines;
            ViewBag.RCode = RCode;
            return View("Index", datasetViewModel);
        }

        public ActionResult LoadPartialView(int partialNumber)
        {
            ViewBag.RCode = RCode;
            switch (partialNumber)
            {
                case 1:
                    return PartialView("_SummaryPartial");
                case 2:
                    return PartialView("_PlotPartial");
                default:                    
                    return PartialView("_RCodePartial", new { RCode });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
