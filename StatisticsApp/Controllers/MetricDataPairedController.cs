using System;
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
    public class MetricDataPairedController : Controller
    {
        public static string WwwrootPath = "C:/Users/Paula/Desktop/FER-10.semestar/" +
            "StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/";
        public static CSharpR CSharpR = new CSharpR(
            "C:/Program Files/R/R-4.0.5/bin/x64/Rscript.exe");
        public static string RScriptPath = "C:/Users/Paula/Desktop/FER-10.semestar/metric_data_paired.r";
        public static string[] RCode = System.IO.File.ReadAllLines(RScriptPath);
        public static string Dataset;
        public static string[] Lines;
        public static List<SelectListItem> AlternativeHypotheses = new List<SelectListItem>()
        {
            new SelectListItem { Text="Jednostrani - manji od (<)", Value="less" },
            new SelectListItem { Text="Jednostrani - veći od (>)", Value="greater" },
            new SelectListItem { Text="Dvostrani", Value="two.sided" }
        };

        public IActionResult Index()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(
                WwwrootPath + "test_plots");
            foreach (FileInfo file in directoryInfo.EnumerateFiles())
            {
                file.Delete();
            }
            PairedViewModel pairedViewModel = new PairedViewModel()
            {
                AlternativeHypothesis = AlternativeHypotheses[0].Text,
                AlternativeHypotheses = AlternativeHypotheses,
                ConfidenceInterval = 0.95
            };
            ViewBag.TestResult = new string[] { "Odaberite parametre testa." };
            ViewBag.RCode = RCode;
            return View("Index", pairedViewModel);
        }

        public IActionResult Test(PairedViewModel pairedViewModel)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(
                WwwrootPath + "test_plots");
            foreach (FileInfo file in directoryInfo.EnumerateFiles())
            {
                file.Delete();
            }
            string[] output = CSharpR.ExecuteRScript(RScriptPath,
                new string[] { Dataset,
                pairedViewModel.AlternativeHypothesis,
                pairedViewModel.ConfidenceInterval.ToString(),
                },
                out string standardError);
            ViewBag.TestResult = output.Skip(4);
            ViewBag.RCode = RCode;
            ViewBag.Dataset = Lines;           
            pairedViewModel.AlternativeHypotheses = AlternativeHypotheses;
            ViewBag.Images = Directory.EnumerateFiles(WwwrootPath + "test_plots")
                 .Select(fn => "~/test_plots/" + Path.GetFileName(fn));
            return View("Index", pairedViewModel);
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
            PairedViewModel pairedViewModel = new PairedViewModel()
            {                
                AlternativeHypothesis = AlternativeHypotheses[0].Text,
                AlternativeHypotheses = AlternativeHypotheses,       
                ConfidenceInterval = 0.95
            };
            ViewBag.TestResult = new string[] { "Odaberite parametre testa." };
            ViewBag.RCode = RCode;
            return View("Index", pairedViewModel);
        }
    }
}