using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StatisticsApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StatisticsApp.Controllers
{
    public class CategoricalDataSingleProportionController : Controller
    {
        public static string WwwrootPath = "C:/Users/Paula/Desktop/FER-10.semestar/" +
            "StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/";
        public static CSharpR CSharpR = new CSharpR(
            "C:/Program Files/R/R-4.0.5/bin/x64/Rscript.exe");
        public static string RScriptPath = "C:/Users/Paula/Desktop/FER-10.semestar/" +
            "categorical_data_single_proportion.r";
        public static string[] RCode = System.IO.File.ReadAllLines(RScriptPath);
        public static string Dataset;
        public static string[] Lines;
        public static List<SelectListItem> Variables;
        public static List<SelectListItem> AlternativeHypotheses = new List<SelectListItem>()
        {
            new SelectListItem { Text="Jednostrani - manji od (<)", Value="less" },
            new SelectListItem { Text="Jednostrani - veći od (>)", Value="greater" },
            new SelectListItem { Text="Dvostrani", Value="two.sided" }
        };

        public IActionResult Index()
        {
            return View();
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
            Variables = new List<SelectListItem>();
            int counter = 1;
            foreach (string variable in Lines[0].Split(",").Select(x => x = x.Replace("\"", "")))
            {
                Variables.Add(new SelectListItem() { Text = variable, Value = counter.ToString() });
                counter++;
            }
            ViewBag.Dataset = Lines;
            SingleProportionViewModel singleProportionViewModel = new SingleProportionViewModel()
            {                
                Variable = Variables[0].Text,
                Variables = Variables,                
                AlternativeHypothesis = AlternativeHypotheses[0].Text,
                AlternativeHypotheses = AlternativeHypotheses,
                ConfidenceInterval = 0.95
            };
            ViewBag.TestResult = new string[] { "Odaberite parametre testa." };
            ViewBag.RCode = RCode;
            return View("Index", singleProportionViewModel);
        }
    }
}