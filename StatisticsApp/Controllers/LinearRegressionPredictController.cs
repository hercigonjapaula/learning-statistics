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
    public class LinearRegressionPredictController : Controller
    {
        public static string WwwrootPath = "C:/Users/Paula/Desktop/FER-10.semestar/" +
            "StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/";
        public static CSharpR CSharpR = new CSharpR(
            "C:/Program Files/R/R-4.0.5/bin/x64/Rscript.exe");
        public static string RScriptPath = "C:/Users/Paula/Desktop/FER-10.semestar/linear_regression_predict.r";
        public static string[] RCode = System.IO.File.ReadAllLines(RScriptPath);
        public static string Dataset;
        public static string DatasetNew;
        public static List<SelectListItem> Variables;
        public static string[] Lines;

        public IActionResult Index()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(
                WwwrootPath + "linreg_plots");
            foreach (FileInfo file in directoryInfo.EnumerateFiles())
            {
                file.Delete();
            }
            LinearRegressionPredictViewModel linRegPredictViewModel =
                new LinearRegressionPredictViewModel()
                {
                    Variables = new List<SelectListItem>()
                };
            ViewBag.Result = new string[] { "Učitajte novi skup podataka." };
            ViewBag.RCode = RCode;
            return View("Index", linRegPredictViewModel);
        }

        [HttpPost]
        public IActionResult ChangeXAndY(LinearRegressionPredictViewModel linRegPredictViewModel)
        {
            string[] output = CSharpR.ExecuteRScript(RScriptPath,
                new string[] { WwwrootPath,
                WwwrootPath + TempData["dataset_name"] as string,
                DatasetNew,
                linRegPredictViewModel.X,
                linRegPredictViewModel.Y
                },
                out string standardError);
            TempData.Keep("dataset_name");
            linRegPredictViewModel.Variables = Variables;
            ViewBag.Result = output.SkipLast(2);
            ViewBag.Images = Directory.EnumerateFiles(WwwrootPath + "linreg_plots")
                 .Select(fn => "~/linreg_plots/" + Path.GetFileName(fn));
            ViewBag.RCode = RCode;
            ViewBag.Dataset = Lines;
            return View("Index", linRegPredictViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(
                WwwrootPath + "linreg_plots");
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
            DatasetNew = WwwrootPath + file.FileName;
            Lines = System.IO.File.ReadAllLines(DatasetNew);
            ViewBag.Dataset = Lines;
            Variables = new List<SelectListItem>();
            int counter = 1;
            foreach (string variable in Lines[0].Split(",").Select(x => x = x.Replace("\"", "")))
            {
                Variables.Add(new SelectListItem() { Text = variable, Value = counter.ToString() });
                counter++;
            }
            LinearRegressionPredictViewModel linRegPredictViewModel = new LinearRegressionPredictViewModel()
            {
                X = Variables[0].Text,
                Y = Variables[0].Text,
                Variables = Variables
            };
            ViewBag.RCode = RCode;
            ViewBag.Result = ViewBag.Result = new string[] { "Odaberite varijable X i Y." };
            return View("Index", linRegPredictViewModel);
        }
    }
}