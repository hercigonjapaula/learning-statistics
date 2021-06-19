using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StatisticsApp.Models;

namespace StatisticsApp.Controllers
{
    public class MetricDataSingleController : Controller
    {
        public static string WwwrootPath = "C:/Users/Paula/Desktop/FER-10.semestar/" +
            "StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/";
        public static CSharpR CSharpR = new CSharpR(
            "C:/Program Files/R/R-4.0.5/bin/x64/Rscript.exe");
        public static string RScriptPath = "C:/Users/Paula/Desktop/FER-10.semestar/metric_data_single.r";
        public static string[] RCode = System.IO.File.ReadAllLines(RScriptPath);
        public static string[] Lines = System.IO.File.ReadAllLines(WwwrootPath + "mtcars.csv");    
        public static string Dataset;
        public static List<SelectListItem> Datasets = new List<SelectListItem>()
        {
                    new SelectListItem() { Text="mtcars", Value="mtcars" },
                    new SelectListItem() { Text="USArrests", Value="USArrests" }                    
        };
        public static List<SelectListItem> Variables;
        public static List<SelectListItem> AlternativeHypotheses = new List<SelectListItem>()
        {
            new SelectListItem { Text="Jednostrani - manji od (<)", Value="less" },
            new SelectListItem { Text="Jednostrani - veći od (>)", Value="greater" },
            new SelectListItem { Text="Dvostrani", Value="two.sided" }
        };
        public static List<SelectListItem> Tests = new List<SelectListItem>()
        {
            new SelectListItem { Text="Srednja vrijednost", Value="mean" },
            new SelectListItem { Text="Varijanca", Value="var" }  
        };

        public IActionResult Index()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(
                WwwrootPath + "test_plots");
            foreach (FileInfo file in directoryInfo.EnumerateFiles())
            {
                file.Delete();
            }
            Variables = new List<SelectListItem>();
            int counter = 1;
            foreach (string variable in Lines[0].Split(",").Select(x => x = x.Replace("\"", "")))
            {
                Variables.Add(new SelectListItem() { Text = variable, Value = counter.ToString() });
                counter++;
            }
            SingleViewModel singleViewModel = new SingleViewModel
            {
                Dataset = "mtcars",
                Datasets = Datasets,
                Variable = Variables[0].Text,
                Variables = Variables,
                Test = Tests[0].Text,
                Tests = Tests,
                AlternativeHypothesis = AlternativeHypotheses[0].Text,
                AlternativeHypotheses = AlternativeHypotheses,
                ConfidenceInterval = "0.95 "              
            };
            Dataset = "mtcars";
            ViewBag.TestResult = "Odaberite parametre testa.";
            ViewBag.Dataset = Lines;
            ViewBag.RCode = RCode;
            return View("Index", singleViewModel);
        }

        public IActionResult Test(SingleViewModel singleViewModel)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(
                WwwrootPath + "test_plots");
            foreach (FileInfo file in directoryInfo.EnumerateFiles())
            {
                file.Delete();
            }
            string[] output = CSharpR.ExecuteRScript(RScriptPath,
                new string[] { WwwrootPath + "test_plots", Dataset, singleViewModel.Variable,
                singleViewModel.NullHypothesis, singleViewModel.AlternativeHypothesis,
                singleViewModel.ConfidenceInterval, singleViewModel.Test },
                out string standardError);
            output = output[0].Trim().Split(" ");
            ViewBag.Images = Directory.EnumerateFiles(WwwrootPath + "test_plots")
                 .Select(fn => "~/test_plots/" + Path.GetFileName(fn));
            ViewBag.Statistic = output[0];
            ViewBag.Df = output[1];
            ViewBag.PValue = output[2];
            ViewBag.ConfInt = "[" + output[3] + ", " + output[4] + "]";
            ViewBag.Estimate = output[5];            
            ViewBag.RCode = RCode;
            ViewBag.Dataset = Lines;                   
            singleViewModel.Variables = Variables;
            singleViewModel.Tests = Tests;
            singleViewModel.AlternativeHypotheses = AlternativeHypotheses;
            singleViewModel.Dataset = Dataset;
            singleViewModel.Datasets = Datasets;
            return View("Index", singleViewModel);
        }

        [HttpPost]
        public IActionResult ChangeDataset(SingleViewModel singleViewModel)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(
                WwwrootPath + "test_plots");
            foreach (FileInfo file in directoryInfo.EnumerateFiles())
            {
                file.Delete();
            }
            Lines = System.IO.File.ReadAllLines(WwwrootPath + singleViewModel.Dataset + ".csv");
            Variables = new List<SelectListItem>();
            int counter = 1;
            foreach (string variable in Lines[0].Split(",").Select(x => x = x.Replace("\"", "")))
            {
                Variables.Add(new SelectListItem() { Text = variable, Value = counter.ToString() });
                counter++;
            }
            Dataset = singleViewModel.Dataset;
            singleViewModel.Datasets = Datasets;
            singleViewModel.Variable = Variables[0].Text;
            singleViewModel.Variables = Variables;
            singleViewModel.Test = Tests[0].Text;
            singleViewModel.Tests = Tests;
            singleViewModel.AlternativeHypothesis = AlternativeHypotheses[0].Text;
            singleViewModel.AlternativeHypotheses = AlternativeHypotheses;
            singleViewModel.ConfidenceInterval = "0.95";     
            ViewBag.TestResult = "Odaberite parametre testa.";
            ViewBag.Dataset = Lines;
            ViewBag.RCode = RCode;
            return View("Index", singleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(
                WwwrootPath + "test_plots");
            foreach (FileInfo f in directoryInfo.EnumerateFiles())
            {
                f.Delete();
            }
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
            Dataset = WwwrootPath + file.FileName;
            Lines = System.IO.File.ReadAllLines(Dataset);
            ViewBag.Dataset = Lines;
            Variables = new List<SelectListItem>();
            int counter = 1;
            foreach (string variable in Lines[0].Split(",").Select(x => x = x.Replace("\"", "")))
            {
                Variables.Add(new SelectListItem() { Text = variable, Value = counter.ToString() });
                counter++;
            }
            SingleViewModel singleViewModel = new SingleViewModel()
            {
                Dataset = Dataset,
                Datasets = Datasets,
                Variable = Variables[0].Text,
                Variables = Variables,
                Test = Tests[0].Text,
                Tests = Tests,
                AlternativeHypothesis = AlternativeHypotheses[0].Text,
                AlternativeHypotheses = AlternativeHypotheses,
                ConfidenceInterval = "0.95"
            };
            ViewBag.TestResult = "Odaberite parametre testa.";
            ViewBag.RCode = RCode;
            return View("Index", singleViewModel);
        }
    }
}