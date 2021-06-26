using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StatisticsApp.Models;

namespace StatisticsApp.Controllers
{
    public class LinearRegressionInferController : Controller
    {
        public static string WwwrootPath = "C:/Users/Paula/Desktop/FER-10.semestar/" +
            "StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/";
        public static CSharpR CSharpR = new CSharpR(
            "C:/Program Files/R/R-4.0.5/bin/x64/Rscript.exe");
        public static string RScriptPath = "C:/Users/Paula/Desktop/FER-10.semestar/linear_regression_infer.r";
        public static string[] RCode = System.IO.File.ReadAllLines(RScriptPath);
        public static string Dataset;      
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
            Dataset = TempData["dataset_linreg_path"] as string;
            Lines = System.IO.File.ReadAllLines(Dataset);
            TempData.Keep();
            Variables = new List<SelectListItem>();
            int counter = 1;
            foreach (string variable in Lines[0].Split(",").Select(x => x = x.Replace("\"", "")))
            {
                Variables.Add(new SelectListItem() { Text = variable, Value = counter.ToString() });
                counter++;
            }
            LinearRegressionInferViewModel linRegInferViewModel = new LinearRegressionInferViewModel()
            {
                X = Variables[0].Text,
                Y = Variables[0].Text,
                Variables = Variables
            };
            ViewBag.Dataset = Lines;
            ViewBag.Result = "Odaberite varijable X i Y.";
            ViewBag.RCode = RCode;
            return View("Index", linRegInferViewModel);
        }

        [HttpPost]
        public IActionResult ChangeXAndY(LinearRegressionInferViewModel linRegInferViewModel)
        {
            string[] output = CSharpR.ExecuteRScript(RScriptPath,
                new string[] { WwwrootPath,
                Dataset,                
                linRegInferViewModel.X,
                linRegInferViewModel.Y
                },
                out string standardError);
            TempData.Keep();
            linRegInferViewModel.Variables = Variables;
            output = output[0].Split(" ");
            ViewBag.RSquared = output[0];
            ViewBag.AdjRSquared = output[1];
            string[] ttest = System.IO.File.ReadAllLines(WwwrootPath + "ttest.txt");
            Regex regex = new Regex(" +");           
            ViewBag.TTestIntercept = regex.Split(ttest[1]).Skip(1).ToArray();
            ViewBag.TTestSlope = regex.Split(ttest[2]).Skip(2).ToArray();
            string[] ftest = System.IO.File.ReadAllLines(WwwrootPath + "ftest.txt");           
            ftest = regex.Split(ftest[1]).ToArray();         
            ViewBag.Images = Directory.EnumerateFiles(WwwrootPath + "linreg_plots")
                 .Select(fn => "~/linreg_plots/" + Path.GetFileName(fn));
            ViewBag.RCode = RCode;
            ViewBag.Dataset = Lines;
            return View("Index", linRegInferViewModel);
        }
    }
}