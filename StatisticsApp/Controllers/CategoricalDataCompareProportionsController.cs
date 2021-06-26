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
    public class CategoricalDataCompareProportionsController : Controller
    {
        public static string WwwrootPath = "C:/Users/Paula/Desktop/FER-10.semestar/" +
            "StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/";
        public static CSharpR CSharpR = new CSharpR(
            "C:/Program Files/R/R-4.0.5/bin/x64/Rscript.exe");
        public static string RScriptPath = "C:/Users/Paula/Desktop/FER-10.semestar/" +
            "categorical_data_compare_proportions.r";
        public static string RScriptLevelsPath = "C:/Users/Paula/Desktop/FER-10.semestar/" +
            "levels.r";
        public static string[] RCode = System.IO.File.ReadAllLines(RScriptPath);
        public static string Dataset;
        public static string[] Lines;
        public static string Variable1;
        public static string Variable2;
        public static List<SelectListItem> Variables;
        public static List<SelectListItem> Levels1;
        public static List<SelectListItem> Levels2;
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
            Dataset = TempData["dataset_categorical_path"] as string;
            TempData.Keep();
            Lines = System.IO.File.ReadAllLines(Dataset);
            Variables = new List<SelectListItem>();
            int counter = 1;
            foreach (string variable in Lines[0].Split(",").Select(x => x = x.Replace("\"", "")))
            {
                Variables.Add(new SelectListItem() { Text = variable, Value = counter.ToString() });
                counter++;
            }
            ViewBag.Dataset = Lines;
            CompareProportionsViewModel compareProportionsViewModel = new CompareProportionsViewModel()
            {
                Variable1 = Variables[0].Text,
                Variable2 = Variables[0].Text,
                Variables = Variables,
                AlternativeHypothesis = AlternativeHypotheses[0].Text,
                AlternativeHypotheses = AlternativeHypotheses
            };
            string[] levels = CSharpR.ExecuteRScript(RScriptLevelsPath,
                new string[] { Dataset,
                compareProportionsViewModel.Variables[0].Value
                },
                out string standardError);
            TempData.Keep();
            Levels1 = new List<SelectListItem>();
            foreach (string level in levels[0].Split(",").Select(x => x = x.Replace("\"", "")))
            {
                Levels1.Add(new SelectListItem() { Text = level, Value = level });
            };
            compareProportionsViewModel.Level1 = Levels1[0].Text;
            compareProportionsViewModel.Level21 = Levels1[0].Text;
            compareProportionsViewModel.Level22 = Levels1[0].Text;
            compareProportionsViewModel.Levels1 = Levels1;
            compareProportionsViewModel.Levels2 = Levels1;
            ViewBag.TestResult = "Odaberite parametre testa.";
            ViewBag.RCode = RCode;
            return View("Index", compareProportionsViewModel);
        }

        [HttpPost]
        public IActionResult ChangeVariable(CompareProportionsViewModel compareProportionsViewModel)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(
                WwwrootPath + "test_plots");
            foreach (FileInfo file in directoryInfo.EnumerateFiles())
            {
                file.Delete();
            }
            Variable1 = compareProportionsViewModel.Variable1;
            Variable2 = compareProportionsViewModel.Variable2;
            string[] levels = CSharpR.ExecuteRScript(RScriptLevelsPath,
                new string[] { Dataset,
                compareProportionsViewModel.Variable1
                },
                out string standardError);
            TempData.Keep();
            Levels1 = new List<SelectListItem>();
            foreach (string level in levels[0].Split(",").Select(x => x = x.Replace("\"", "")))
            {
                Levels1.Add(new SelectListItem() { Text = level, Value = level });
            };
            levels = CSharpR.ExecuteRScript(RScriptLevelsPath,
                new string[] { Dataset,
                compareProportionsViewModel.Variable2
                },
                out string stdErr);
            TempData.Keep();
            Levels2 = new List<SelectListItem>();
            foreach (string level in levels[0].Split(",").Select(x => x = x.Replace("\"", "")))
            {
                Levels2.Add(new SelectListItem() { Text = level, Value = level });
            };
            compareProportionsViewModel.AlternativeHypothesis = AlternativeHypotheses[0].Text;
            compareProportionsViewModel.AlternativeHypotheses = AlternativeHypotheses;
            compareProportionsViewModel.Variables = Variables;            
            compareProportionsViewModel.Level1 = Levels1[0].Text;
            compareProportionsViewModel.Level21 = Levels2[0].Text;
            compareProportionsViewModel.Level22 = Levels2[0].Text;
            compareProportionsViewModel.Levels1 = Levels1;
            compareProportionsViewModel.Levels2 = Levels2;            
            ViewBag.TestResult = "Odaberite parametre testa.";
            ViewBag.RCode = RCode;
            ViewBag.Dataset = Lines;
            return View("Index", compareProportionsViewModel);
        }

        [HttpPost]
        public IActionResult ChangeLevel(CompareProportionsViewModel compareProportionsViewModel)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(
                WwwrootPath + "test_plots");
            foreach (FileInfo file in directoryInfo.EnumerateFiles())
            {
                file.Delete();
            }
            compareProportionsViewModel.AlternativeHypotheses = AlternativeHypotheses;
            compareProportionsViewModel.Variables = Variables;
            compareProportionsViewModel.Levels1 = Levels1;
            compareProportionsViewModel.Levels2 = Levels2;            
            string[] output = CSharpR.ExecuteRScript(RScriptPath,
                new string[] { WwwrootPath + "test_plots",
                Dataset,
                Variable1,
                Variable2,
                compareProportionsViewModel.Level1,
                compareProportionsViewModel.Level21,
                compareProportionsViewModel.Level22,                
                compareProportionsViewModel.AlternativeHypothesis                
                },
                out string standardError);
            TempData.Keep();
            output = output[0].Trim().Split(" ");
            ViewBag.Statistic = output[0];
            ViewBag.Df = output[1];
            ViewBag.PValue = output[2];
            ViewBag.ConfInt = "[" + output[3] + ", " + output[4] + "]";
            ViewBag.Estimate = "(" + output[5] + ", " + output[6] + ")";
            ViewBag.RCode = RCode;
            ViewBag.Dataset = Lines;
            ViewBag.Images = Directory.EnumerateFiles(WwwrootPath + "test_plots")
                 .Select(fn => "~/test_plots/" + Path.GetFileName(fn));
            return View("Index", compareProportionsViewModel);
        }
    }
}