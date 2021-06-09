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
        public static string WwwrootPath = "C:/Users/Paula/Desktop/FER-10.semestar/" +
                    "StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/";
        public static CSharpR CSharpR = new CSharpR("C:/Program Files/R/R-4.0.5/bin/x64/Rscript.exe");
        public static string[] Lines = System.IO.File.ReadAllLines(WwwrootPath + "iris.csv");
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
        public static string RScriptPath = "C:/Users/Paula/Desktop/FER-10.semestar/basics.r";
        public static string[] RCode = System.IO.File.ReadAllLines(RScriptPath);


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
            DirectoryInfo directoryInfo = new DirectoryInfo(
                WwwrootPath + "basic_plots");
            foreach (FileInfo file in directoryInfo.EnumerateFiles())
            {
                file.Delete();
            }
            string[] output = CSharpR.ExecuteRScript(RScriptPath,
                new string[] { datasetViewModel.Dataset, datasetViewModel.Variable, datasetViewModel.Plot },
                out string standardError);
            output = CSharpR.ExecuteRScript("C:/Users/Paula/Desktop/FER-10.semestar/summary.r",
                new string[] { datasetViewModel.Dataset, datasetViewModel.Variable, datasetViewModel.Plot },
                out string stdError);
            string[] lines = System.IO.File.ReadAllLines(
                WwwrootPath + "summary.txt");
            string[] summary = lines[1].Trim().Split("   ");
            ViewBag.Min = summary[0];
            ViewBag.FirstQ = summary[1];
            ViewBag.Median = summary[2];
            ViewBag.Mean = summary[3];
            ViewBag.ThirdQ = summary[4];
            ViewBag.Max = summary[5];
            ViewBag.Dataset = Lines;          
            ViewBag.RCode = RCode;
            ViewBag.RunRCode = false;
            ViewBag.BasicPlots = Directory.EnumerateFiles(WwwrootPath + "basic_plots")
                            .Select(fn => "~/basic_plots/" + Path.GetFileName(fn));
            TempData["dataset_name"] = Dataset;
            TempData.Keep();
            return View(datasetViewModel);
        }
  
        [HttpPost]
        public IActionResult ChangeDataset(DatasetViewModel datasetViewModel)
        {            
            Dataset = datasetViewModel.Dataset;
            Lines = System.IO.File.ReadAllLines(WwwrootPath  
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
            DirectoryInfo directoryInfo = new DirectoryInfo(
                WwwrootPath + "basic_plots");
            foreach (FileInfo file in directoryInfo.EnumerateFiles())
            {
                file.Delete();
            }
            string[] output = CSharpR.ExecuteRScript(RScriptPath,
                new string[] { datasetViewModel.Dataset, datasetViewModel.Variable, datasetViewModel.Plot },
                out string standardError);
            output = CSharpR.ExecuteRScript("C:/Users/Paula/Desktop/FER-10.semestar/summary.r",
                new string[] { datasetViewModel.Dataset, datasetViewModel.Variable, datasetViewModel.Plot },
                out string stdError);
            string[] lines = System.IO.File.ReadAllLines(
                WwwrootPath + "summary.txt");
            string[] summary = lines[1].Trim().Split("   ");
            ViewBag.Min = summary[0];
            ViewBag.FirstQ = summary[1];
            ViewBag.Median = summary[2];
            ViewBag.Mean = summary[3];
            ViewBag.ThirdQ = summary[4];
            ViewBag.Max = summary[5];
            ViewBag.Dataset = Lines;
            ViewBag.RCode = RCode;
            ViewBag.RunRCode = false;
            ViewBag.BasicPlots = Directory.EnumerateFiles(WwwrootPath + "basic_plots")
                .Select(fn => "~/basic_plots/" + Path.GetFileName(fn));
            TempData["dataset_name"] = Dataset;
            TempData.Keep();
            return View("Index", datasetViewModel);
        }

        [HttpPost]
        public IActionResult ChangeVariableAndPlot(DatasetViewModel datasetViewModel)
        {
            datasetViewModel.Datasets = Datasets;
            datasetViewModel.Dataset = Dataset;
            datasetViewModel.Variables = Variables;
            datasetViewModel.Plots = Plots;
            DirectoryInfo directoryInfo = new DirectoryInfo(
                WwwrootPath + "basic_plots");
            foreach (FileInfo file in directoryInfo.EnumerateFiles())
            {
                file.Delete();
            }
            string[] output = CSharpR.ExecuteRScript(RScriptPath,
                new string[] { datasetViewModel.Dataset, datasetViewModel.Variable, datasetViewModel.Plot }, 
                out string standardError);
            output = CSharpR.ExecuteRScript("C:/Users/Paula/Desktop/FER-10.semestar/summary.r",
                new string[] { datasetViewModel.Dataset, datasetViewModel.Variable, datasetViewModel.Plot },
                out string stdError);
            string[] lines = System.IO.File.ReadAllLines(
                WwwrootPath + "summary.txt");
            string[] summary = lines[1].Trim().Split("   ");
            ViewBag.Min = summary[0];
            ViewBag.FirstQ = summary[1];
            ViewBag.Median = summary[2];
            ViewBag.Mean = summary[3];
            ViewBag.ThirdQ = summary[4];
            ViewBag.Max = summary[5];
            ViewBag.Dataset = Lines;
            ViewBag.RCode = RCode;
            ViewBag.RunRCode = false;
            ViewBag.BasicPlots = Directory.EnumerateFiles(WwwrootPath + "basic_plots")
                .Select(fn => "~/basic_plots/" + Path.GetFileName(fn));
            return View("Index", datasetViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Content("File not selected");
            }
            var path = Path.Combine(WwwrootPath + 
                    file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            Lines = System.IO.File.ReadAllLines(WwwrootPath
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
            Dataset = WwwrootPath +
                    file.FileName;
            DirectoryInfo directoryInfo = new DirectoryInfo(
                WwwrootPath + "basic_plots");
            foreach (FileInfo f in directoryInfo.EnumerateFiles())
            {
                f.Delete();
            }
            string[] output = CSharpR.ExecuteRScript(RScriptPath,
                new string[] { Dataset, datasetViewModel.Variable, datasetViewModel.Plot },
                out string standardError);
            output = CSharpR.ExecuteRScript("C:/Users/Paula/Desktop/FER-10.semestar/summary.r",
                new string[] { datasetViewModel.Dataset, datasetViewModel.Variable, datasetViewModel.Plot },
                out string stdError);
            string[] lines = System.IO.File.ReadAllLines(
                WwwrootPath + "summary.txt");
            string[] summary = lines[1].Trim().Split("   ");
            ViewBag.Min = summary[0];
            ViewBag.FirstQ = summary[1];
            ViewBag.Median = summary[2];
            ViewBag.Mean = summary[3];
            ViewBag.ThirdQ = summary[4];
            ViewBag.Max = summary[5];
            ViewBag.Dataset = Lines;
            ViewBag.RCode = RCode;
            ViewBag.RunRCode = false;
            ViewBag.BasicPlots = Directory.EnumerateFiles(WwwrootPath + "basic_plots")
                .Select(fn => "~/basic_plots/" + Path.GetFileName(fn));
            TempData["dataset_name"] = file.FileName.Replace(".csv", "");
            TempData.Keep();
            return View("Index", datasetViewModel);
        }

        [HttpPost]
        public IActionResult RunRCode (IFormCollection formFields)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(
                WwwrootPath + "rcode_plots");
            foreach(FileInfo file in directoryInfo.EnumerateFiles())
            {
                file.Delete();
            }
            string rcode = formFields["rcode"];
            string path = WwwrootPath + "code.r";
            System.IO.File.WriteAllText(path, rcode);
            DatasetViewModel datasetViewModel = new DatasetViewModel
            {
                Dataset = Dataset,
                Datasets = Datasets,
                Variable = Variables[0].Value,
                Variables = Variables,
                Plot = Plots[0].Value,
                Plots = Plots
            };
            CSharpR.ExecuteRScript(path,
                new string[] { datasetViewModel.Dataset, datasetViewModel.Variable, datasetViewModel.Plot },
                out string stdError);
            ViewBag.Images = Directory.EnumerateFiles(WwwrootPath + "rcode_plots")
                             .Select(fn => "~/rcode_plots/" + Path.GetFileName(fn));
            string[] lines = System.IO.File.ReadAllLines(
                WwwrootPath + "summary.txt");
            string[] summary = lines[1].Trim().Split("   ");
            ViewBag.Min = summary[0];
            ViewBag.FirstQ = summary[1];
            ViewBag.Median = summary[2];
            ViewBag.Mean = summary[3];
            ViewBag.ThirdQ = summary[4];
            ViewBag.Max = summary[5];
            ViewBag.Dataset = Lines;
            ViewBag.RCode = RCode;
            ViewBag.RunRCode = true;
            ViewBag.BasicPlots = Directory.EnumerateFiles(WwwrootPath + "basic_plots")
                .Select(fn => "~/basic_plots/" + Path.GetFileName(fn));
            return View("Index", datasetViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
