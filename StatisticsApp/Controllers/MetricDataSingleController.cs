using System.Collections.Generic;
using System.Linq;
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
        public static string[] Lines;
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
            if (TempData.ContainsKey("dataset_name"))
            {
                Lines = System.IO.File.ReadAllLines(
                    WwwrootPath 
                    + TempData["dataset_name"] as string + ".csv");
                TempData.Keep("dataset_name");
            }
            Variables = new List<SelectListItem>();
            int counter = 1;
            foreach (string variable in Lines[0].Split(",")
                .Select(x => x = x.Replace("\"", "")))
            {
                Variables.Add(new SelectListItem() { Text = variable, Value = counter.ToString() });
                counter++;
            }
            SingleViewModel singleMeanViewModel = new SingleViewModel()
            {
                Variable = Variables[0].Text,
                Variables = Variables,
                Test = Tests[0].Text,
                Tests = Tests,
                AlternativeHypothesis = AlternativeHypotheses[0].Text,
                AlternativeHypotheses = AlternativeHypotheses,
                ConfidenceInterval = 0.95
            };
            ViewBag.TestResult = new string[] { "Odaberite parametre testa." };
            ViewBag.Dataset = Lines;
            ViewBag.RCode = RCode;
            return View("Index", singleMeanViewModel);
        }

        public IActionResult Test(SingleViewModel singleViewModel)
        {           
            string[] output = CSharpR.ExecuteRScript(RScriptPath,
                new string[] { TempData["dataset_name"] as string, singleViewModel.Variable,
                singleViewModel.NullHypothesis.ToString(), singleViewModel.AlternativeHypothesis,
                singleViewModel.ConfidenceInterval.ToString(), singleViewModel.Test },
                out string standardError);
            ViewBag.TestResult = output;
            ViewBag.RCode = RCode;
            ViewBag.Dataset = Lines;
            TempData.Keep("dataset_name");            
            singleViewModel.Variables = Variables;
            singleViewModel.Tests = Tests;
            singleViewModel.AlternativeHypotheses = AlternativeHypotheses;
            return View("Index", singleViewModel);
        }
    }
}