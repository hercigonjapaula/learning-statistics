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
    public class CategoricalDataIndependenceController : Controller
    {
        public static string WwwrootPath = "C:/Users/Paula/Desktop/FER-10.semestar/" +
            "StatisticsApp/StatisticsApp/StatisticsApp/wwwroot/";
        public static CSharpR CSharpR = new CSharpR(
            "C:/Program Files/R/R-4.0.5/bin/x64/Rscript.exe");
        public static string RScriptPath = "C:/Users/Paula/Desktop/FER-10.semestar/" +
            "categorical_data_independence.r";
        public static string[] RCode = System.IO.File.ReadAllLines(RScriptPath);
        public static string Dataset;
        public static string[] Lines;
        public static List<SelectListItem> Variables1;
        public static List<SelectListItem> Variables2;
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
            Variables1 = new List<SelectListItem>();
            Variables2 = new List<SelectListItem>();
            int counter = 1;
            foreach (string variable in Lines[0].Split(",").Select(x => x = x.Replace("\"", "")))
            {
                Variables1.Add(new SelectListItem() { Text = variable, Value = counter.ToString() });
                Variables2.Add(new SelectListItem() { Text = variable, Value = counter.ToString() });
                counter++;
            }
            ViewBag.Dataset = Lines;
            IndependenceViewModel independenceViewModel = new IndependenceViewModel()
            {
                Variable1 = Variables1[0].Text,
                Variable2 = Variables2[0].Text,
                Variables1 = Variables1,
                Variables2 = Variables2,
                AlternativeHypothesis = AlternativeHypotheses[0].Text,
                AlternativeHypotheses = AlternativeHypotheses
            };
            ViewBag.TestResult = "Odaberite parametre testa.";
            ViewBag.RCode = RCode;
            return View("Index", independenceViewModel);
        }

        public IActionResult Test(IndependenceViewModel independenceViewModel)
        {
            string[] output = CSharpR.ExecuteRScript(RScriptPath,
                new string[] { WwwrootPath,
                    Dataset,
                    independenceViewModel.Variable1,
                    independenceViewModel.Variable2,
                    independenceViewModel.AlternativeHypothesis
                },
                out string standardError);
            TempData.Keep();
            output = output[0].Trim().Split(" ");
            ViewBag.NumOfSucc = output[0];
            ViewBag.NumOfTrials = output[1];
            ViewBag.PValue = output[2];
            ViewBag.RCode = RCode;
            ViewBag.Dataset = Lines;
            ViewBag.Images = Directory.EnumerateFiles(WwwrootPath + "test_plots")
                 .Select(fn => "~/test_plots/" + Path.GetFileName(fn));
            string[] table = System.IO.File.ReadAllLines(WwwrootPath + "contingency_table.txt");
            ViewBag.ColNames = table[1].Trim().Split(" ");
            List<string> rowNames = new List<string>();
            Dictionary<string, List<string>> rows = new Dictionary<string, List<string>>();
            Regex regex = new Regex(" +");            
            foreach (string line in table.Skip(2))
            {
                string[] row = regex.Split(line.Trim());
                string rowName = row[0];
                rowNames.Add(rowName);
                rows[rowName] = row.Skip(1).ToList();
            }
            ViewBag.RowNames = rowNames;
            ViewBag.Rows = rows;
            independenceViewModel.Variables1 = Variables1;
            independenceViewModel.Variables2 = Variables2;           
            independenceViewModel.AlternativeHypotheses = AlternativeHypotheses;
            return View("Index", independenceViewModel);
        }    
    }
}