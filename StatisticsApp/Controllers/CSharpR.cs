using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StatisticsApp.Controllers
{
    public class CSharpR
    {
        public readonly string RExeFile;

        public CSharpR(string rExeFile)
        {
            RExeFile = rExeFile;
        }

        public string[] ExecuteRScript(string RScriptFile, string[] args,
            out string standardError)
        {
            List<string> output = new List<string>();
            string[] outputLines = new string[0];            
            string outputText = string.Empty;
            standardError = string.Empty;
            try
            {
                using (Process process = new Process())
                {                    
                    process.StartInfo = new ProcessStartInfo(RExeFile)
                    {
                        Arguments = string.Format("{0} {1}", RScriptFile, string.Join(" ", args)),
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true                        
                    };
                    process.Start();
                    while (!process.StandardOutput.EndOfStream)
                    {
                        output.Add(process.StandardOutput.ReadLine());
                    }
                    outputLines = output.ToArray();
                    standardError = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message;
            }
            return outputLines;
        }
    }
}
