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
    public class MetricDataCompareController : Controller
    {
        public static string WwwrootPath = "C:/Users/Paula/Desktop/FER-10.semestar/" +
            "StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/";
        public static CSharpR CSharpR = new CSharpR(
            "C:/Program Files/R/R-4.0.5/bin/x64/Rscript.exe");
        public static string RScriptPath = "C:/Users/Paula/Desktop/FER-10.semestar/metric_data_compare.r";
        public static string[] RCode = System.IO.File.ReadAllLines(RScriptPath);
        public static string Dataset1;
        public static string Dataset2;
        public static string[] Lines1;
        public static string[] Lines2;
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
            CompareViewModel compareViewModel = new CompareViewModel()
            {                
                AlternativeHypothesis = AlternativeHypotheses[0].Text,
                AlternativeHypotheses = AlternativeHypotheses,
                Test = Tests[0].Text,
                Tests = Tests,
                ConfidenceInterval = "0.95"
            };
            ViewBag.TestResult = "Odaberite parametre testa.";            
            ViewBag.RCode = RCode;
            return View("Index", compareViewModel);
        }

        public IActionResult Test(CompareViewModel compareViewModel)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(
                WwwrootPath + "test_plots");
            foreach (FileInfo file in directoryInfo.EnumerateFiles())
            {
                file.Delete();
            }
            string[] output = CSharpR.ExecuteRScript(RScriptPath,
                new string[] { WwwrootPath + "test_plots",
                Dataset1,
                Dataset2,
                compareViewModel.AlternativeHypothesis,
                compareViewModel.ConfidenceInterval,
                compareViewModel.Test },
                out string standardError);
            output = output[2].Trim().Split(" ");
            ViewBag.Statistic = output[0];
            if (compareViewModel.Test == "mean")
            {
                ViewBag.Df = output[1];
                ViewBag.PValue = output[2];
                ViewBag.ConfInt = "[" + output[3] + ", " + output[4] + "]";
                ViewBag.Estimate = "(" + output[5] + ", " + output[6] + ")";
            }
            else
            {
                ViewBag.Df = "(" + output[1] + ", " + output[2] + ")";
                ViewBag.PValue = output[3];
                ViewBag.ConfInt = "[" + output[4] + ", " + output[5] + "]";                                                               
                ViewBag.Estimate = output[6];                
            }                       
            ViewBag.RCode = RCode;
            ViewBag.Dataset1 = Lines1;
            ViewBag.Dataset2 = Lines2;                       
            compareViewModel.Tests = Tests;
            compareViewModel.AlternativeHypotheses = AlternativeHypotheses;
            ViewBag.Images = Directory.EnumerateFiles(WwwrootPath + "test_plots")
                 .Select(fn => "~/test_plots/" + Path.GetFileName(fn));
            return View("Index", compareViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileFirst(IFormFile file)
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
            Dataset1 = WwwrootPath + file.FileName;
            Lines1 = System.IO.File.ReadAllLines(Dataset1);
            ViewBag.Dataset1 = Lines1;
            ViewBag.Dataset2 = Lines2;
            CompareViewModel compareViewModel = new CompareViewModel()
            {
                Dataset1 = Dataset1,
                AlternativeHypothesis = AlternativeHypotheses[0].Text,
                AlternativeHypotheses = AlternativeHypotheses,
                Test = Tests[0].Text,
                Tests = Tests,
                ConfidenceInterval = "0.95"
            };
            ViewBag.TestResult = "Odaberite parametre testa.";
            ViewBag.RCode = RCode;
            return View("Index", compareViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileSecond(IFormFile file)
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
            Dataset2 = WwwrootPath + file.FileName;
            Lines2 = System.IO.File.ReadAllLines(Dataset2);
            ViewBag.Dataset2 = Lines2;
            ViewBag.Dataset1 = Lines1;
            CompareViewModel compareViewModel = new CompareViewModel()
            {
                Dataset1 = Dataset1,
                Dataset2 = Dataset2,
                AlternativeHypothesis = AlternativeHypotheses[0].Text,
                AlternativeHypotheses = AlternativeHypotheses,
                Test = Tests[0].Text,
                Tests = Tests,
                ConfidenceInterval = "0.95"
            };
            ViewBag.TestResult = "Odaberite parametre testa.";
            ViewBag.RCode = RCode;
            return View("Index", compareViewModel);
        }
    }
}